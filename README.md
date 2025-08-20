## About the Module

Evaluation Forms is a component of the **casc-vel** product. It lets supervisors create evaluation forms with criteria to assess operator performance. Supervisors define and group criteria and configure scoring rules to compute the final score. After an evaluation is completed, a supervisor can publish the result for the operator to review. Operators can review the result and, if needed, dispute the evaluation.

## Architecture

### C4 - Context
```mermaid
C4Context
  title System Context for Evaluations Module in casc-vel
  
  Person_Ext(customer, "Customer", "Contacts the company via communication channels")

  System_Ext(Channels, "Communication Channels", "Chat, voice, email, social and other entry points")

  System_Boundary(cascvel, "casc-vel Platform") {
    Boundary(WebArmB, "WebArm", "boundary") {
      System(WebArmShell, "WebArm Shell", "Host for microfrontends")
      System(Evaluations, "Evaluations Module", "Form authoring, criteria grouping, scoring rules, evaluation runs, publishing and disputes")
    }
    System(Backend, "casc-vel Backend Services", "Modular backend services and APIs for domain capabilities")
  }

  Person(operator, "Operator", "Handles customer interactions and reviews evaluation results")
  Person(supervisor, "Supervisor", "Evaluates operators using predefined forms and publishes outcomes")
  Person(designer, "Evaluation Form Designer", "Creates evaluation forms and criteria")
  

  Rel(customer, Channels, "Contacts company")
  BiRel(Channels, Backend, "Conversations/events")
  
  Rel(WebArmShell, Evaluations, "Hosts UI")
  BiRel(WebArmShell, Backend, "Shared APIs")
  BiRel(Evaluations, Backend, "APIs/events")

  Rel(operator, WebArmShell, "Handles work")
  Rel(supervisor, WebArmShell, "Runs evaluations")
  Rel(designer, WebArmShell, "Designs forms")

  UpdateLayoutConfig($c4ShapeInRow="2")
```

