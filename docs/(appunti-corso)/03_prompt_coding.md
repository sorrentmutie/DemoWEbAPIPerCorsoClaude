 Dobbiamo creare un progetto seguendo il clean code per le entità di dominio. Quello che vogliamo fare è gestire un endpoint per i prodotti

 Aggiungiamo la documentazione dell'API con swagger e swashbuckle. Modifica il program.cs in modo da avviarla assieme all'API

 dobbiamo aggiungere delle regole di business. il prezzo del prodotto non può superare i 1000€ e il nome del prodotto deve essere di lunghezza inferiore a 20 caratteri

 Fai un po di refactoring. Anzitutto usa MapGroup e poi classi statiche per le funzioni da utilizzare negli endpoint

 inseriamo la persistenza, vorrei usare EF core in memoria. crea tutto il codice necessario isolandolo dal progetto API. ultrathink

 Dobbiamo modificare il dominio. Un prodotto appartiene a una categoria. Ci serve anche l'endpoint completo delle categorie. Tieni conto di una relazione uno a molti

 puoi usare un DTO per i prodotti? Vorrei vedere restituito dall'API il nome della categoria

 crea un dto per le categorie restituendo per ogni categoria il numero di prodotti

 aggiungiamo un progetto per gli unit test della logina che hai implementato. evidenzia tutti gli edge cases sia per prodotti che per categorie. ultrathink