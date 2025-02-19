**MediFlow.Demo**

**Apraksts**

Jebkurš vadītājs un jebkurš darbinieks var pievienot jaunu klientu sistēmai, un katra darbinieku grupa, piemēram medmāsas un aprūpētāji var veidot vienu ierakstu dienā katram klientam.
Visi pagājušie ieraksti ir tikai lasāmi, un tās pašas dienas ietvaros ieraksts ir pilnībā labojams.
Papildus aprūpētāji nevar skatīt medmāsas ierakstus, bet medmāsa var skatīt aprūpētājas ierakstus, un vadītāja var skatīt visus ierakstus.

Vadītājs var pārslēgties no vienas struktūrvienības uz citu no jebkuras ierīces bez problēmām.
Drošības nodrošināšanai darbinieki var pieslēgties sistēmai tikai no darba iekārtām, kas tiek nodrošināts izmantojot Cookie. 

Pieslēdzoties sistēmai ir trīs opcijas:
* Login - kad darbinieks jau ir daļa no struktūrvienības
* Join - kad darbiniekam ir profils, bet viņš vēl nav daļa no struktūrvienības
* Register - kad darbienikam vēl nav profila

**Kā lietot**
Šis ir projekts, kas ļauj vadītājam pārvaldīt vairākas struktūrvienības, un darbiniekus un klientus tajās.

Lai palaistu projektu ieteicams izmantot metodi ar docker izmantošanu:
Nepieciešams atvērt termināli attiecīgajā direktorijā un izpildīt doto komandu:
`docker-compose up`
Tas var aizņemt kādu laiku.
Alternatīvi, ir iespējams uzsākt katru projektu atsevišķi, vienīgais ir nepieciešama PostgreSQL datubāze api darbībai.

Sākotnēji nepieciešams pieslēgties sistēmai kā administratoram un izveidot pirmo vadītāju.
admina epasts: mediflow.noreply@gmail.com
admina parole: P@55w0rd

Kā admins pievienojam vienu lietotāju, kas būs vadītājs (Manager)

Tālāk pieslēdzamies kā vadītājs, izmantojot iepriekš norādīto epastu un paroli
Pārvietojamies uz struktūras pārvaldības sadaļu izmantojot rīkjoslu kreisajā malā vai sekojot dotajai adresei http://localhost:3000/structure/manage.
Sākumā ierīci padaram par darba ierīci, spiežot "Pievienot ierīci"
Tālāk kā vadītājs varam pievienot gan klientus, gan izveidot ielūgumus darbiniekiem.

Lai izveidotu ielūguma, nepieciešams spiest uz "Izveidot ielūgumu", un norādīt darbinieka amatu.
Tālāk no jebkuras darba iekārtas darbinieks var pieslēgties sistēmai izmantojot attiecīgo ielūgumu, un tiks automātiski pievienots attiecīgajā struktūrā un amatā.

Labkus rīkjoslai ir klientu saraksts, un spiežot "Pievienot klientu" ir iespējams pievienot jaunus klientus.

Par katru klientu ir pieejamas divas sadaļas:
* personas dati
* Žurnāls - kas ir dienas ierakstu kopums

Žurnālā ir iespēja atfiltrēt ierakstus pēc datumu diapazona, ierakstu autora un karodziņa, kas apzīmē svarīgus ierakstus.
