# Authority

Identity and user management for .NET applications.

## Core Concepts

### Domain

Domain is the top level element inside Authority. Inside a domain users are unique, identified by their email address. However users can exists within different domains with the same email. Authority has two modes for handling domains:

  - Single mode: there are exactly one domain inside the application. If at the initialization of Authority there is no domain in the database then Authority automatically create one.
  - Multi mode: there can be multiple domain. The initialization of Authority does not create a domain if none exists.

### Claim

Claims are basically key value pairs and they can be converted to the Claim class of the System.Security namespace.

### Policy

Policies are exist within Domains and hold zero or more claims. Users can be assigned to policies. Inside a Domain there can be zero or one default policy which automatically assigned to newly registered users (however if a new default policy appointed existing users do not get this new default policy). Policies belong zero or more Group. Policies have zero or more Claims.

### Group

Groups are exists within Domains and can be assigned to Users. A Group itself does not hold any Claims but zero or more Policy can be assigned to a Group. Just like Policies Groups can get the default flag. A default Group operates the same way as a default Policy.

### User

Users are our main point of interest. In Authority users exists within a Domain uniquely identified by their email address. Beside that their username also unique too.
All user has zero or more Groups and/or Policies and Users can own Claims too (the Claims the User own and the Claims that the User own through its Policies are the User's owned Claims). There are different strategies inside Authority to aquire the Claims of a User.
Users can be converted to a ClaimsPrincipal instance through an extension method.
