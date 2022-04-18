#TODO

##Make connection string username/password use environmental variables. Then docker-compose can just give the same username/password to backend/db

##Convert transaction types incoming to snake_case_like_this.

Reject anything not entirley composed of alphabet, ' ', '_', or '-'
Convert all alphabet to lowercase
Convert all ' ' to '_'
Leave '-' as is

Front end will convert back as it likes