# CsharpBasketballTournamentSimulation

## Info
Ovaj program simulira olimpijski košarkaški turnir

Početna forma tima se izračunava iz exibitions.json fajla, a kasnije povećava kada tim pobedi

Umesto izračunavanja koji će tim pobediti, pravi se nasumičan broj od 70 do 90 postignutih koševa i za svaki od njih izračunava verovatnoću koji je tim dao koš, i time se pravi realan finalni rezultat.

### Pravila grupne faze su:
1. Grupna faza se sastoji od toga da svaki tim igra sa preostala tri tima iz svoje grupe. Timovi dobijaju:
   - 2 boda za pobedu,
   - 1 bod za poraz,
   - 0 bodova za poraz predajom.
2. Timovi u okviru grupe se rangiraju na osnovu broja bodova.
U slučaju da dva tima iz iste grupe imaju isti broj bodova, rezultat međusobnog susreta će biti korišćen kao kriterijum za rangiranje.
U slučaju da 3 tima iz iste grupe imaju isti broj bodova, kriterijum za rangiranje biće razlika u poenima u međusobnim utakmicama između ta 3 tima (takozvano formiranje kruga).
3. Na kraju grupne faze prvoplasirani, drugoplasirani i trećeplasirani timovi iz svih grupa dobijaju rang od 1 do 9:
   - Prvoplasirani timovi iz grupa A, B i C se medjusobno rangiraju po primarno po broju bodova, zatim koš razlici (u slučaju jednakog broja bodova) i zatim broja postignutih koševa (u slučaju jednakog broja bodova i koš razlike) kako bi im se dodelili rangovi 1, 2 i 3.
   - Drugoplasirani timovi iz grupa A, B i C se medjusobno rangiraju istom principu kako bi im se dodelili rangovi  4, 5 i 6.
   - Trećeplasirani timovi iz grupa A, B i C se medjusobno rangiraju po istom principu kako bi im se dodelili rangovi  7, 8 i 9.
4. Ekipe sa rangom od 1 do 8 prolaze u eliminacionu fazu, ekipa sa rangom 9 ne nastavlja takmičenje.

## Žreb
Timovi koji su se kvalifikovali u četvrtfinale biće podeljeni u četiri šešira:
   - Šešir D: Timovi sa rangom 1 i 2.
   - Šešir E: Timovi sa rangom 3 i 4.
   - Šešir F: Timovi sa rangom 5 i 6.
   - Šešir G: Timovi sa rangom 7 i 8.

Timovi iz šešira `D` se nasumično ukrštaju sa timovima iz šešira `G`, a timovi iz šešira `E` sa timovima iz šešira `F` i tako se formiraju parovi četvrtfinala. Veoma važna propozicija je da se timovi koji su igrali međusobno u grupnoj fazi, ne mogu sresti u četvrtfinalu.

U istom trenutku se formiraju i parovi polufinala, nasumičnim ukrštanjem novonastalih parova četvrtfinala, uz pravilo da se parovi nastali ukrštanjem šešira `D i E` ukrštaju sa parovima nastalim ukrštanjem šešira `F i G`.

## Eliminaciona faza
Turnir se nastavlja standardnim formatom eliminacije, gde pobednici prolaze u polufinale, a gubitnici ispadaju. Pobednici polufinala idu u finale, dok gubitnici igraju za bronzanu medalju.
