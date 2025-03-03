# [Domain Driven Design][YP]

### Contents
0. [YouTube Play List][YP] | [Summarry][V0]
1. [Video 1][Y1] | [DDD][100]
    + [Initial Create][110]
        + [Add All Projects to soultion][111]
        + [Add Projects Refernce][112]
        + [Add Package][113]
    + [Setting Up Login][120]
        + [Set Up Dependency Per Project][121]
        + [Code Flow][122]
2. [Video 2][Y2] | [JWT][2]
    + [Creating Model for appsettings section][21]
    + [User Secrets][22]

3. [Video 3][Y3] | [Repository Pattern][3]
    + [Version 2][31]

4. [Video 4][Y4] | [Global Error Handling][4]
    + [Via Middelware][41]
    + [Using Filters][42]
    + [Using ProblemDetails][43]
    + [Using Controller End Point][44]
    + [Error Handling Flow][45]

5. [Video 5][Y5] | [Flow Control][5]
    + [Error Handling Via Exeption][51]
    + [Error Handling Using OneOf][52]
    + [Error Handling Using FluentResults][53]
    + [Error Handling Using ErrorOr][54]

6. [Video 6][Y6] | CQRS and [MediatR][6]
    + [MediatR][61]

7. [Video 7][Y7] | [Mapster][7]
    + [Mapster in Code][71]

8. [Video 8][Y8] | [Model Validation][8]
    + [Add Fluent Validation][81]
    + [Generic Validator][82]
    + [Fix messages][83]

    + Refernce* [Validation Attributes][Y80] 

9. [Video 9][Y9] | [JWT Bearer Authentication][9]
    + [Adding Authorization][91]
    + [Seeting Authorization Globally][92]

10. [Video 10][Y10] | Event Storming YouTube Video
    + [Event Storming YouTube Video][Y10p]. We will use draw.io instead

11. [Watch this First][Y11p] | [Video 11][Y11] | Mapping Software Logic Using Process Modeling YouTube Video | [Output][V11]
```
    To recap:
    1. Identify Entities and treat each entity as an aggregate root
    2. Identify relationships between the entities
    3. Merge aggregates if there are constraints

    Possible constraints:
    1. Enforcing Invariants
    2. Eventual consistency cannot be tolerated

    Good indicators that an entity should be an aggregate root:
    1. It is referenced by other aggregates
    2. It will be looked up by Id
```
12. [Video 12][Y12] | [Implementing AggregateRoot Entity ValueObject][V12] 

13. [Video 13][Y13] | [Domain Layer Structure & Skeleton  Clean Architecture][V13] 
14. [Video 14][Y14] | [REST + DDD + CA + CQRS  When it all plays together][V14] 
15. [Video 15][Y15] | [EF Core DDD and Clean Architecture  Mapping Aggregates to Relational Databases][V15]
16. [Video 16][Y16] | [The Identity Paradox  DDD EF Core & Strongly Typed IDs][V16]

[100]:Docs/v/Video1-0.md
[110]:Docs/v/Video1-1-setup.md#intial-creation
[111]:Docs/v/Video1-1-setup.md#add-all-created-projects-in-solution
[112]:Docs/v/Video1-1-setup.md#add-project-refernce
[113]:Docs/v/Video1-1-setup.md#add-package
[120]:Docs/v/Video1-2-Setting-Up-Login.md#setting-up-login
[121]:Docs/v/Video1-2-Setting-Up-Login.md#setup-dependency-injection-per-project
[122]:Docs/v/Video1-2-Setting-Up-Login.md#code-flow

[2]:Docs/v/Video2-JWT.md#jwt
[21]:Docs/v/Video2-JWT.md#creating-model-for-appsettings-section
[22]:Docs/v/Video2-JWT.md#user-secret

[3]:Docs/v/Video3.md
[31]:Docs/v/Video3-v2.md

[4]:Docs/v/Video4.md#error-handling
[41]:Docs/v/Video4.md#via-middelware
[42]:Docs/v/Video4.md#via-exception-filter-attribute
[43]:Docs/v/Video4.md#using-problemdetails
[44]:Docs/v/Video4.md#via-error-endpoint
[45]:Docs/v/Video4.md#error-handling-flow

[5]:Docs/v/Video5-0.md
[51]:Docs/v/Video5-1-Via-Exception.md
[52]:Docs/v/Video5-2-OneOf.md
[53]:Docs/v/Video5-3-FluentResults.md
[54]:Docs/v/Video5-4-ErrorOr.md

[6]:Docs/v/Video6-CQRS.md#cqrs
[61]:Docs/v/Video6-CQRS.md#mediatr

[7]:Docs/v/Video7-1-Mapster.md
[71]:Docs/v/Video7-2-MapsterInCode.md

[8]:Docs/v/Video8-ModelValidation.md#model-validation
[81]:Docs/v/Video8-ModelValidation.md#add-fluent-validation
[82]:Docs/v/Video8-ModelValidation.md#convert-to-a-generic-validator
[83]:Docs/v/Video8-ModelValidation.md#fix-title-of-error-response
[84]:Docs/v/Video8-ModelValidation.md#base-controller-clean-up
[85]:Docs/v/Video8-ModelValidation.md#using-the-generic-validator

[9]:Docs/v/Video9.md#bearer-authentication
[91]:Docs/v/Video9.md#adding-authorization
[92]:Docs/v/Video9.md#adding-authorization-global---to-inheriting-classes

[V11]:Docs/Api/DomainModels/Aggregates.Menu.md
[V12]:Docs/v/Video12.md#implementing-aggregateroot-entity-valueobject
[V13]:Docs/v/Video13.md#domain-layer-structure--skeleton--clean-architecture
[V14]:Docs/v/Video14.md#rest--ddd--ca--cqrs--when-it-all-plays-together
[V15]:Docs/v/Video15.md#ef-core-ddd-and-clean-architecture--mapping-aggregates-to-relational-databases
[V16]:Docs/v/Video16.md#the-identity-paradox--ddd-ef-core--strongly-typed-ids

[V0]:Docs/v/_Summary.md#domain-driven-design---codes-summary

[YP]:https://www.youtube.com/playlist?list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k
[Y1]:https://www.youtube.com/watch?v=fhM0V2N1GpY
[Y2]:https://www.youtube.com/watch?v=38bQNWKh0dk
[Y3]:https://www.youtube.com/watch?v=ZwQf_JQUUCQ
[Y4]:https://www.youtube.com/watch?v=gMwAhKddHYQ
[Y5]:https://www.youtube.com/watch?v=tZ8gGqiq_IU
[Y6]:https://www.youtube.com/watch?v=MwMVvLBSJa8
[Y7]:https://www.youtube.com/watch?v=vBs6naPD6RE
[Y8]:https://www.youtube.com/watch?v=FXP3PQ03fa0
[Y80]:https://www.youtube.com/watch?v=-ix1hzWr2ws
[Y9]:https://www.youtube.com/watch?v=7ILCRfPmQxQ
[Y10p]:https://www.youtube.com/watch?v=7LFxWgfJEeI
[Y10]:https://www.youtube.com/watch?v=1pBGc7kKOAA
[Y11p]:https://www.youtube.com/watch?v=UEtmOW8uZZY
[Y11]:https://www.youtube.com/watch?v=f6G46rqkePc

[Y12]:https://www.youtube.com/watch?v=weGLBPky43U
[Y13]:https://www.youtube.com/watch?v=jnutb5Z4wyg
[Y14]:https://www.youtube.com/watch?v=jm0CWlb5vvQ
[Y15]:https://www.youtube.com/watch?v=5_un3PUER8U
[Y16]:https://www.youtube.com/watch?v=B3Iq346KwUQ
