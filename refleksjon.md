# Oppgave 3: Drone Race

## Del A

Kjørte programmet først i to terminaler for å sammenligne.

```
Drone Tiny Timmy reached checkpoint 17
Drone Tiny Timmy reached checkpoint 18
Drone Tiny Timmy reached its final destination
Drone Big Berta reached checkpoint 10
Drone Big Berta reached checkpoint 11
Drone Big Berta reached checkpoint 12
Drone Big Berta reached its final destination
Race is finished
```

```
Drone Tiny Timmy reached checkpoint 16
Drone Tiny Timmy reached checkpoint 17
Drone Big Berta reached checkpoint 11
Drone Tiny Timmy reached checkpoint 18
Drone Tiny Timmy reached its final destination
Drone Big Berta reached checkpoint 12
Drone Big Berta reached its final destination
Race is finished
```

Når man sender noe til en delt ressurs som console så kan rekkefølgen på utførelse bli
litt tilfeldig, som man ser av utskriften over.

Hvis man fjerner join så får man i begynnelsen:

```
Race is starting
Race is finished
Drone Tiny Timmy started its run.
Drone Big Berta started its run.
```

Normalt ved et funksjonskall så går programtråden over til funksjonen og
kjører instruksjonene der for så å returnere.
Ved kall til Thread.Start() så startes det en ny tråd hvor funksjonen utføres og
tråden som kallet Start() fortsetter sin egen kjøring umiddelbart.
Det er derfor "Race is finished" blir skrevet ut før dronene har startet sitt kjør.

## Del B

## Del C

## Del D
