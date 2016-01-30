# Revlucio.NtmlAuthentication

An implementation of the NTML authentication protocol as a self-contained ASP.NET Core 1.0 Middleware.

Protocol description:
http://www.innovation.ch/personal/ronald/ntlm.html

Basically it goes:

REQUEST ->
<- NEGOTIATE
REQUEST W/ NAME ->
<- CHALLENGE
REQUEST W/ AUTH HEADER ->
<- RESPONSE

And at any other point:
REQUEST /W AUTH HEADER ->
<- RESPONSE