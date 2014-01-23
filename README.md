NHibernate by example
============================

# Part 1 - The pit of success

## Sesssion management

  - JUST ONE SESSION FACTORY (per tenant ;) )

  - Scoping! There's lots of information on the web, but the right answer is to use your container.
  - Session per request
  - using flush (don't call flush explicitly!)
  - not committing transactions
    
## Modeling

  - Map your aggregate roots correctly
  
## Mapping

  - Make sure you get the right Id pattern
  - Increment is SINGLE INSTANCE ONLY.
  - Beware enums and dirty flags
  - Yes, you really have to mark everything as virtual, including unrelated methods

## Understanding the NHibernate cache

## Talking about repositories

  - NHibernate is one, so don't wrap it.

## Querying

  - Query over
  - ICriteria
  - Linq
  - Encapsulate queries, not "ISession", CQRS-ish.
  

# Part 2 - Querying, code hygine, advanced topics 
 
## N+1 and other foibles

  - Beware lazy loading - user "Fetch"
  - But don't disable lazy loading  
  
# Advanced Queries

  - Fetch modes
  - Projections
  - HQL
  - Multi-queries / Futures / Batching
  
# Advanced mapping

  - Composite type mappings make your ugly tables a little nicer
  - Mapping expressions
 
# Interceptors and events

  - Use the framework to bridge the gulf of model / storage details (encryption, weird data mapping problems)