# NOR_WAY webapplikasjoner

# Del 1
  # Forutsetninger i prosjektet:
    - Henter første rute i databasen, ikke nødvendigvis den raskeste
    - Kortnummer, Utløpsdato og CVC har ingen andre annen funksjon enn at man ikke får lov å 
      utføre bestillingen før man har fylt dem ut med "gyldig" informasjon. 
      
# Del 2
  # Forutsetninger i prosjektet:
    - Admin har ikke mulighet til å oppdatere eller opprette Ordre og Ordrelinjer
    - Ved sletting av en Rute blir alle tilhørende Avganger, Ordre og Ordrelinjer også slettet 
    - Ved sletting av en Avgang blir alle tilhørende Ordre og Ordrelinjer også slettet
    - Admin har ikke mulighet til å slette en billettype, da rader i Ordrelinjer-tabell avhenger av dem
    
    # Admin innlogging: 
      Brukernavn: Admin
      Passord: Admin123
    
    # E-postadresser brukt til seeding av ordre:
      [ ola@nodmann.no, kari@nordmann.no, john@doe.no, jane@doe.no, peder@aas.no, lars@holm.no ]
    
    
    
