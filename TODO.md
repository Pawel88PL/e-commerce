1. Utworzyć rolę administratora dla zarządzania magazynem i zamówieniami.

    - zabezpieczyć magazyn, dodawanie, usuwanie i edycję produktów przed nieautoryzowanym dostępem. --> ZROBIONE
    
2. Umożliwić klientom założenie konta.

    - utworzyć formularz rejestracji ze wszystkimi potrzebnymi polami, --> ZROBIONE
    - zadbać o walidację wprowadzanych danych --> ZROBIONE
    - formularz logowania, żeby pamiętał poprzedni, wpisany adres email
    - dodać potwierdzenie wpisanego hasła, --> ZROBIONE
    - dodać funkcję wysyłającą link do aktywacji konta po pomyślnym zarejestrowaniu użytkownika, --> ZROBIONE
    - ustawić autofocus na pierwsze pole w formularzu rejestracji i logowania, --> ZROBIONE

3. Powiadomić użytkownika o gromadzeniu danych osobowych i używaniu plików cookie, uzyskać zgodę. --> ZROBIONE

4. Rozszerzenie API kontrolera koszyka o dodatkowe endpointy:

    - pobieranie zawartości koszyka. --> ZROBIONE
    - aktualizowanie ilości produktu w koszyku. --> ZROBIONE
    - usuwanie produktu z koszyka. --> ZROBIONE
    - czyszczenie koszyka.

5. Utworzyć moduł zamówień.

    - powiązać cartId z userId po pomyślnym zalogowaniu użytkownika, --> ZROBIONE
    - po naciśnięciu przycisku 'Przejdź do kasy' przekierowanie do strony z wyborem opcji: 'Mam już konto' i 'Jestem nowym użytkownikiem',
      a dla zalogowanego użytkownika przekierowanie do strony z potwierdzeniem zamówienia, --> ZROBIONE

6. W API utworzyć serwis odpowiadający za wysyłanie wiadomości email:
    - metodę wysyłającą email do klienta po pomyślnym zarejestrowaniu się przenieść do serwisu 'EmailService.cs', --> ZROBIONE
    - przenieść dane logowania konta mailowego do pliku appsetings.json, --> ZROBIONE

7. Utworzyć moduł klienta:
    - umożliwić klientom przeglądanie i edycję swoich danych --> ZROBIONE
    - możliwość zmianę hasła --> ZROBIONE
    - przeglądanie swoich zamówień --> ZROBIONE

8. Utworzyć moduł do zarządzania zamówieniami przez Admina --> ZROBIONE.
9. Usunąć 'inCheckoutProcess' po pomyślnym złożeniu zamówienia --> ZROBIONE
10. Umożliwić zmianę hasła dla użytkownika, który zapomniał swojego hasła.
11. Umożliwić złożenie zamówienia jako gość.
12. W formularzu logowania przenieść pole 'Hasło' obok pola 'Potwierdź hasło' --> ZROBIONE
13. Dodać datę rejestracji użytkownika w bazie danych --> ZROBIONE
14. Umożliwić administratorowi wgląd w bazę zarejestrowanych użytkowników --> ZROBIONE
15. Dodać formatowanie pola 'Telefon' i 'Mail' w formularzu rejestracji --> ZROBIONE