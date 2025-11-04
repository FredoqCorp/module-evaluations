# JWT Authentication Integration Guide

This guide explains how to integrate the Evaluations Module with your identity provider using JWT Bearer authentication.

## Overview

The Evaluations Module uses **JWT Bearer tokens** for authentication and authorization. The module validates tokens issued by your identity provider but does not handle user authentication itself.

### Key Concepts

- **Stateless**: The module doesn't maintain session state
- **Claims-based**: User information is extracted from JWT token claims
- **Role-based**: Three module-specific roles control access to features
- **Flexible**: Works with any OAuth2/OpenID Connect compliant identity provider

## Module Roles

The module defines three roles that map to business functions:

| Role | Description | Example Permissions |
|------|-------------|-------------------|
| **FormDesigner** | Creates and manages evaluation forms | View forms list, create/edit/delete forms |
| **Supervisor** | Evaluates operator performance | View forms, submit evaluations, view results |
| **Operator** | Contact center agent being evaluated | View own evaluation results (future feature) |

## Configuration

### 1. Module Configuration

Configure the module's JWT authentication in `appsettings.json`:

```json
{
  "JwtBearer": {
    "Authority": "https://your-identity-provider.com",
    "Audience": "evaluations-module"
  }
}
```

**Configuration Parameters:**

- `Authority`: Your identity provider's base URL (must support OpenID Connect discovery at `/.well-known/openid-configuration`)
- `Audience`: The audience claim value that the module expects in tokens (configured in your identity provider)

### How JWT Signature Validation Works

The module **automatically validates JWT signatures** using public keys from your identity provider:

1. **Identity Provider publishes public keys** at `{Authority}/.well-known/openid-configuration` (OpenID Connect Discovery)
2. **Module downloads JWKS (JSON Web Key Set)** containing RSA/ECDSA public keys on startup
3. **For each incoming request**, the module:
   - Extracts the `kid` (key ID) from JWT header
   - Finds matching public key in cached JWKS
   - Validates cryptographic signature using the public key
   - Verifies issuer, audience, and expiration
   - Extracts claims only if signature is valid

**Security Model:**
- Identity Provider signs tokens with **private key** (kept secret)
- Module validates signatures with **public key** (downloaded from JWKS endpoint)
- You do NOT need to manually configure signing keys - they are discovered automatically via `Authority`

**Example Discovery Flow:**
```
Module Config: "Authority": "https://keycloak.company.com/realms/mycompany"

1. Module fetches: https://keycloak.company.com/realms/mycompany/.well-known/openid-configuration
   Response: { "jwks_uri": "https://keycloak.company.com/realms/mycompany/protocol/openid-connect/certs", ... }

2. Module fetches: https://keycloak.company.com/realms/mycompany/protocol/openid-connect/certs
   Response: { "keys": [{ "kty": "RSA", "kid": "abc123", "n": "...", "e": "AQAB" }] }

3. Module caches public keys and uses them to validate all incoming JWT signatures
```

### 2. Identity Provider Configuration

Configure your identity provider to issue JWT tokens with the required claims.

#### Required Claims

| Claim | Type | Description | Example |
|-------|------|-------------|---------|
| `sub` | string | User unique identifier | `"user-12345"` or `"a1b2c3d4-..."` |
| `module_role` | string | Module role name | `"FormDesigner"`, `"Supervisor"`, or `"Operator"` |

#### Optional Claims

| Claim | Type | Description | Fallback |
|-------|------|-------------|----------|
| `preferred_username` | string | Username/login identifier | Falls back to `username` claim |
| `username` | string | Alternative username claim | None if both absent |
| `name` | string | User's full display name | Composed from `given_name` + `family_name` |
| `given_name` | string | User's first name | Used with `family_name` if `name` absent |
| `family_name` | string | User's last name | Used with `given_name` if `name` absent |
| `email` | string | User's email address | None - optional metadata |

#### Example JWT Payload

```json
{
  "sub": "user-12345",
  "module_role": "FormDesigner",
  "preferred_username": "ipetrov",
  "name": "Иван Петров",
  "given_name": "Иван",
  "family_name": "Петров",
  "email": "ivan.petrov@company.com",
  "aud": "evaluations-module",
  "iss": "https://your-identity-provider.com",
  "exp": 1697654321,
  "iat": 1697650721
}
```

## Identity Provider Examples

### Azure AD (Entra ID)

#### 1. Register Application

1. Navigate to Azure Portal → Azure Active Directory → App registrations
2. Create new registration: "Evaluations Module"
3. Note the **Application (client) ID** - this will be your `Audience`

#### 2. Configure App Roles

Add custom app roles in the app manifest:

```json
{
  "appRoles": [
    {
      "allowedMemberTypes": ["User"],
      "description": "Form designers can create and manage evaluation forms",
      "displayName": "Form Designer",
      "id": "a1b2c3d4-...",
      "isEnabled": true,
      "value": "FormDesigner"
    },
    {
      "allowedMemberTypes": ["User"],
      "description": "Supervisors can evaluate operators",
      "displayName": "Supervisor",
      "id": "e5f6g7h8-...",
      "isEnabled": true,
      "value": "Supervisor"
    },
    {
      "allowedMemberTypes": ["User"],
      "description": "Operators are contact center agents",
      "displayName": "Operator",
      "id": "i9j0k1l2-...",
      "isEnabled": true,
      "value": "Operator"
    }
  ]
}
```

#### 3. Add Claims Mapping

Configure Azure AD to map app roles to `module_role` claim:

1. Navigate to Enterprise Applications → Your App → Single sign-on
2. Edit "Attributes & Claims"
3. Add new claim:
   - Name: `module_role`
   - Source: Attribute
   - Source attribute: `user.assignedroles`

#### 4. Module Configuration

```json
{
  "JwtBearer": {
    "Authority": "https://login.microsoftonline.com/{tenant-id}/v2.0",
    "Audience": "{application-client-id}"
  }
}
```

### Keycloak

#### 1. Create Client

1. Navigate to your realm → Clients → Create
2. Client ID: `evaluations-module`
3. Client Protocol: `openid-connect`
4. Access Type: `bearer-only` or `public`

#### 2. Create Client Roles

1. Navigate to Clients → evaluations-module → Roles
2. Add roles: `FormDesigner`, `Supervisor`, `Operator`

#### 3. Create Protocol Mapper

Map client roles to `module_role` claim:

1. Navigate to Clients → evaluations-module → Mappers → Create
2. Configuration:
   - Name: `module-role-mapper`
   - Mapper Type: `User Client Role`
   - Client ID: `evaluations-module`
   - Token Claim Name: `module_role`
   - Claim JSON Type: `String`

#### 4. Module Configuration

```json
{
  "JwtBearer": {
    "Authority": "https://keycloak.company.com/realms/{realm-name}",
    "Audience": "evaluations-module"
  }
}
```

### IdentityServer4 / Duende IdentityServer

#### 1. Define API Resource

```csharp
new ApiResource("evaluations-module", "Evaluations Module")
{
    UserClaims = new[]
    {
        JwtClaimTypes.Subject,
        JwtClaimTypes.Name,
        JwtClaimTypes.GivenName,
        JwtClaimTypes.FamilyName,
        JwtClaimTypes.Email,
        "module_role"
    }
}
```

#### 2. Configure Client

```csharp
new Client
{
    ClientId = "web-app",
    AllowedGrantTypes = GrantTypes.Code,
    AllowedScopes = { "evaluations-module" },
    Claims = new[]
    {
        new ClientClaim("module_role", "FormDesigner") // or from user claims
    }
}
```

#### 3. Add Custom Claim

Implement `IProfileService` to add `module_role` claim:

```csharp
public class CustomProfileService : IProfileService
{
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = // ... get user from database
        var role = // ... get user's module role

        context.IssuedClaims.Add(new Claim("module_role", role));

        return Task.CompletedTask;
    }
}
```

#### 4. Module Configuration

```json
{
  "JwtBearer": {
    "Authority": "https://identity.company.com",
    "Audience": "evaluations-module"
  }
}
```

## Client Integration

### Making Authenticated Requests

Include the JWT token in the `Authorization` header with every request:

```http
GET /forms HTTP/1.1
Host: api.company.com
Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Example: JavaScript/TypeScript

```typescript
const token = await getAccessToken(); // from your auth library

const response = await fetch('https://api.company.com/forms', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});

if (response.status === 401) {
  // Token invalid or expired - refresh token
} else if (response.status === 403) {
  // User doesn't have required role
} else if (response.ok) {
  const forms = await response.json();
}
```

### Example: C# HttpClient

```csharp
using var client = new HttpClient();
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

var response = await client.GetAsync("https://api.company.com/forms");

if (response.StatusCode == HttpStatusCode.Unauthorized)
{
    // Token invalid or expired
}
else if (response.StatusCode == HttpStatusCode.Forbidden)
{
    // User doesn't have required role
}
else if (response.IsSuccessStatusCode)
{
    using var stream = await response.Content.ReadAsStreamAsync();
    using var document = await JsonDocument.ParseAsync(stream);
    var forms = document.RootElement.GetProperty("forms");
}
```

## Troubleshooting

### 401 Unauthorized

**Possible causes:**
- Token is missing from Authorization header
- Token is expired
- Token signature is invalid (wrong key, tampered token, or key rotation)
- Authority URL is incorrect in configuration
- Identity provider's `.well-known/openid-configuration` is unreachable
- JWKS endpoint is unreachable or returns invalid keys

**Solutions:**
1. Verify token is included in request: `Authorization: Bearer {token}`
2. Check token expiration (`exp` claim)
3. Verify `Authority` matches your identity provider's base URL
4. Test discovery endpoint: `{Authority}/.well-known/openid-configuration`
5. Test JWKS endpoint (from discovery response `jwks_uri`)
6. Check network connectivity to identity provider
7. Verify token `kid` (key ID) matches one of the keys in JWKS
8. If keys were rotated, restart the module to refresh cached keys

### 403 Forbidden

**Possible causes:**
- User doesn't have the required `module_role` claim
- `module_role` claim value doesn't match role name exactly (case-sensitive)
- User has wrong role for the endpoint

**Solutions:**
1. Decode JWT token and verify `module_role` claim exists
2. Verify role name matches exactly: `FormDesigner`, `Supervisor`, or `Operator`
3. Check user has been assigned correct role in identity provider
4. Verify endpoint requires correct role (e.g., `/forms` requires `FormDesigner`)

### Invalid Audience

**Error:** `IDX10214: Audience validation failed`

**Solutions:**
1. Verify `Audience` in `appsettings.json` matches `aud` claim in token
2. Configure your identity provider to include correct audience
3. For multiple audiences, identity provider must include `evaluations-module` in `aud` array

### Missing Claims

**Error:** User information not available in application

**Solutions:**
1. Verify `sub` claim is present in token (required)
2. Check identity provider is configured to include optional claims (`name`, `email`)
3. Verify claim mapping configuration in identity provider

## Security Considerations

### HTTPS

**Production:** Always use HTTPS for all communication. The module enforces `RequireHttpsMetadata = true` in production.

**Development:** HTTPS validation is disabled in development environment for local testing.

### Token Lifetime

Configure appropriate token expiration in your identity provider:
- **Access tokens**: 15-60 minutes (shorter is more secure)
- **Refresh tokens**: Hours to days (for seamless user experience)

### Clock Skew

The module allows 5 minutes of clock skew for token validation to handle minor time differences between servers.

### Scope

The module validates tokens but does not enforce API scopes. Implement scope-based authorization if needed using ASP.NET Core's authorization policies.

## Testing

### Generate Test Token (Development Only)

For development testing, you can create test JWT tokens using [jwt.io](https://jwt.io):

```json
{
  "sub": "test-user-123",
  "module_role": "FormDesigner",
  "name": "Test User",
  "email": "test@example.com",
  "aud": "evaluations-module-dev",
  "iss": "https://localhost:5001",
  "exp": 9999999999,
  "iat": 1697650721
}
```

⚠️ **Warning:** Test tokens are for development only. Never use them in production.

### Testing with cURL

```bash
curl -H "Authorization: Bearer YOUR_TOKEN_HERE" \
     https://api.company.com/forms
```

### Testing with Postman

1. Create new request
2. Set Authorization type to "Bearer Token"
3. Paste JWT token in Token field
4. Send request

## Support

For questions or issues with JWT authentication integration:

1. Review [ADR: JWT-Based Authorization](../architecture/adr/20251014-jwt-based-authorization-with-module-roles.md)
2. Check module documentation in repository README
3. Contact your identity provider's support for token configuration issues
4. Open an issue in the module's repository for module-specific problems
