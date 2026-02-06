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

Task og TaskComletionSource er definitivt mer kode enn Threads.
Denne bruken av rekusive metoder for loop og funksjoner som argumenter til
andre funksjoner blir fort litt mer komplisert å holde rede på og reflektere rundt.

Det at man fange exceptions og sende beskjed oppover er definitivt en fordel.

## Del C

Async/Await resulterer i betydlig mindre kode som skrives nesten helt likt som synkron kode.
Exception handling fungerer som ved synkron kode.
Dette gjør at det er lettere å reflekter rundt koden, noe som igjen gir mulighet for en mer
vedlikeholdbar kode.

## Del D
