# www.miododstaniula.pl

Projekt "Miód od Staniula" jest to sklep internetowy, w którym można zamówić polski i prawdziwy miód pszczeli.
Aplikacja sklepu internetowego została napisana w techonologi ASP.NET Core MVC 6.0.
Do połączenia z bazą danych MS SQL Server wykorzystano narzędzie ORM Entity Framework Core.
<hr/>

Chronologia zmian:

Data 24.04.2023

1. Utworzono pierwszą migrację dla tabeli produkty.
2. Utworzono ProductsController odpowiedzialny za dodawanie, edytowanie i usuwanie produktów.
3. Utworzono StoreController do wyświetlania listy produktów.
4. Utworzono widoki dla powyższych kontrolerów.
5. Przeprowadzono update bazy danych.
6. Utworzono widok kart produktów.
7. Dodano kolumnę Photo w tabeli Products.
8. Utworzono link do szczegółów danego produktu klikając na kartę ze zdjęciem.
9. Utworzono przekierowanie do strony ProductsList z kontrolera Edit i Details.
<hr/>

Data 28.04.2023

1. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Magazyn.
2. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Dodaj nowy produkt.
3. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Edytuj produkt.
4. Dodano produkty do sklepu.
5. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Produkty.
6. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Szczegóły produktu.
7. Przeniesiono widok "Szczegóły produktu" do kontrolera StoreController.
8. W widoku "Edytuj produkt" dodano podgląd strony produktu.
9. Poprawiono wygląd, funkcjonalność i wyświetlanie strony Usuń produkt.
<hr/>

Data 29.04.2023

1. Dodano możliwość rejestracji użytkownika.
2. Zabezpieczono kontroler ProductsController metodą autentykacji użytkownika.
3. Dostosowano wyświetlanie strony do urządzeń mobilnych.
4. Poprawiono stopkę.
<hr/>

Data 26.06.2023

1. Dodano klasę Cart.cs reprezentującą model koszyka zakupowego.
2. Zmieniono background dla listy produktów z szarego na białe.
3. Przyciski paska nawigacyjnego zmieniono na btn-outline, dodano ikony bootstrap icons.
<hr/>

Data 23.07.2023

1. Dodano nową klasę Category.cs reprezentującą kategorie danego produktu.
2. Poprawiono pola klasy Product.cs i dodano klucz obcy do tabeli Categories.
3. Przeprowadzono migrację 'newTable_Categories'.
4. Opublikowano wersję aplikacji 2.0.0.
5. Poprawiono wyświetlanie stopki.
6. ConnectionString do bazy danych przeniesiono do pliku appsettings.json, który wyłączono ze śledzenia w GitHub.
7. Utworzono kontroler 'AddNewProductController', klasę 'AddProductService.cs' z metodą 'AddNewProductAsync()'.
<hr/>

Data 24.07.2023

1. Utworzono kontroler 'WarehouseController', klasę 'WarehouseService.cs' z metodą 'GetAllProductsAsync()'.
2. Utworzono kontroler 'EditProductController', klasę 'EditProductService.cs' z metodami 'GetProductAsync()' i 'UpdateProductAsync()'.
<hr/>

Data 25.07.2023

1. W pliku 'Program.cs' skonfigurowano domyślny routing na stronę z produktami ('Store/Index').
2. Utworzono widok dzielony '_WelcomePage', który umieszczono na stronie głównej i na początku strony z produktami.
3. Poprawiono responsywność kart produktów na stronie z produktami.
4. Poprawiono wyświetlanie strony 'Details.cshtml' na urządzeniach mobilnych.
5. Kontroler 'StoreController' zmieniono na 'ProductController' i utworzono w nim medoty filtrowania i sortowania produktów
	według określonych warunków.
6. Na stronie z produktami dodano możliwość filtowania i sortowania produktów.
<hr/>

Data 27.07.2023

1. Dodano plik 'favicon.png' i odnośnik do ikony w tagu "head" pliku 'Layout.cshtml'.
2. Utworzono widok częściowy '_ProductList.cshtml' oraz metodę'GetProducts' w kontrolerze 'ProductsController', która jest odpowiedzialna za przekazywanie
	widoku częściowego do strony z produktami.
3. w pliku 'site.js' zastosowano żądanie AJAX w przypadku filtrowania produktów.
<hr/>

Data 28.07.2023

1. Dla karty produktu dodano przycisk 'Dodaj do koszyka' i efekt hover po najechaniu kursorem na kartę produktu.
2. Utworzono modal dla szczegółów produktu.
3. Zaimportowano czcionkę 'Playfair Display'.
<hr/>
Data 29.07.2023

1. Dodano model klienta 'Client.cs' i koszyka 'Cart.cs'.
2. Przeprowadzono migrację i update bazy danych.
<hr/>

Data 30.07.2023

1. Utworzono klasę 'DeleteServis.cs', interfejs 'IDeleteService.cs' i kontroler 'DeleteController.cs'.
2. Utworzono klasę 'FileUploadService.cs' i odpowiadający mu interfejs. Klasa ta odpowiada za dołączanie zdjęć
	produktów w trakcie dodawania nowego produktu.
<hr/>

Data 31.07.2023

	Utworzono nowy projekt MiodOdStaniula.Tests.
1. W modelu 'Cart.cs' pole 'CartId' i 'ClientId' zmieniono na typ Guid.
2. Zmieniono wyświetlanie paska nawigacyjnego na zawsze widoczny.
3. Dodano projekt biblioteki klas MiodOdStaniula.Domain.
<hr/>

Data 02.08.2023

	Przeniesiono projekt 'MiodOdStaniula.Tests' i 'MiodOdStaniula.Domain.'
1. Usunięto tym samym problem ze śledzeniem tych projektów przez Git.
<hr/>

Data 04.08.2023

	Z sukcesem zaimplementowano funkcjonalność dodawania produktu do koszyka.
1. Utworzono 'CartController', 'CartService' i interfejs 'ICartService'.
2. W projekcie 'MiodOdStaniula.Tests' napisano test dla metody 'AddItemToCart' - test zaliczony.
3. Utworzono gałąź 'version_5.0.0'.
4. Utworzono metodę, która zlicza produkty w koszyku i wyświetla ilość na pasku nawigacyjnym.
5. Zmieniono kolor przycisków 'Dodaj do koszyka' na czarne.
6. Dodano widok "_CartModal".
<hr/>

Data 05.08.2023

	Utworzono modal '_CartModal.cshtml', który pojawia się po dodaniu produktu do koszyka.
1. Dodano kod javascript do pliku 'cart.js', który aktualizuje '_CartModal.cshtml' i stan aktualnego koszyka. 
	Modal ten jest udostępniony jako Partial View w folderze 'Shared'.
2. Utworzono widok koszyka.
<hr/>

Data 06.08.2023

	Utworzono serwis odpowiadający za obliczanie całkowitej kwoty koszyka.
1. Utworzono 'TotalCostService.cs', zaimplementowano w nim metody obliczające koszt wysyłki, koszt produktów
	i podsumowujące całkowity koszty koszyka.
2. Utworzono metodę 'UpdateCartItemQuantityAsync', która aktualizuje ilość danego produktu w koszyku.
3. W pliku 'cart.js' i 'ProductDetails.css' napisano funkcję, która odpowiada za wysuwanie się i chowanie modala
	po dodaniu produktu do koszyka.
<hr/>

Data 08.08.2023

	Przeprowadzono refaktoryzację skryptu 'cart.js'.
1. Poprawiono wyświetlanie widoku 'Details.cshtml'.
2. Poprawiono wygląd strony logowania.
3. Zmieniono całkowicie wygląd sekcji 'footer'.
4. W wioduku 'Warehouse' zmieniono sposób wyświetlania danych - zastosowano tabelę.
<hr/>

Data 09.08.2023

	W widoku współdzielonym '_WelcomePage.cshtml' dodano karuzelę zdjęć.
<hr/>

Data 10.08.2023
	
	Utworzno możliwość dodawania kilku zdjęć dla danego produktu.
1. Utworzono tabelę 'ProductImages', przeprowadzono migrację i update bazy danych.
2. Dla każdego produktu utworzono karuzelę zdjęć.
<hr/>

Data 12.08.2023

	Dodano funkcję 'DeleteImage', która usuwa zdjęcie produktu.
<hr/>

Data 13.08.2023

	Usunięto właściwość 'PhotoUrlAddress' z modelu 'Product'.
1. Przeprowadzono migrację i update bazy danych.
2. Zdjęcia produktu są dostępne jako relacja jeden do wielu z tabelą 'ProductImages'
<hr/>

Data 15.08.2023

	Obsłużono sytuację, w której klient chce dodać produkt, który jest sprzedany.
1. W kontrolerze 'AddNewProduct' dodano polę, które wstawia datę utworzenia nowego produktu.
2. Dla kauzeli zdjęć z '_WelcomePage' ustalono maksymalną szerokość na 1600 px.
3. Do tabeli 'ShopingCart' dodano pole 'CreateCartDate', przeprowadzono migrację i update bazy danych.

<h3>Opublikowano wersję 5.0.0</h3>
<hr/>

Data 17.08.2023

	Poprawiono błąd, który uniemożliwiał filtrowanie produktów.
1. Utworzono kontroler 'CheckoutController', serwis 'CheckoutSerwis', interfejs 'ICheckoutSerwis'
	i folder 'Checkout' z widokiem 'Index.cshtml'.
<hr/>

Data 18.09.2023

	Utworzono klasę 'Order.cs', 'OrderDetail.cs'.
1. Przeprowadzono migrację i update bazy danych.
2. Utworzono widok 'Tunel.cshtm' i 'Index.cshtml' w folderze 'Checkout'.
3. W serwisie 'CheckoutService.cs' utworzono metodę 'CreateOrderAsync()'.
<hr/>

Data 20.09.2023

	Utworzono formularz dodania nowego klienta do bazy danych.
1. Utworzono serwis 'CustomerService.cs' i interfejs 'ICustomerService.cs', a w nich metody dodające
	nowego klienta do bazy danych.
<hr/>

Data 21.09.2023

	Utworzono kontroler bazowy 'BaseController'.
1. W kontrolerze 'BaseController' utworzono metodę 'PrepareCartViewModel()', która przekazuje widok koszyka
	do metod w 'CartController' i 'CheckoutContoller'.
2. W kontrolerze 'BaseController' utworzono metodę 'PrepareProductViewModel()', która przekazuje widok produktu.
<hr/>

Data 22.09.2023

	Przycisk 'Przejdź do kasy' ustawiono jako 'disabled' kiedy nie ma produktów w koszyku.
1. 