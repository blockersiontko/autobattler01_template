# AUTOBATTLER TEMPLATE!

# Arena Duel - Unity Combat Simulator

Prosty projekt w Unity (URP) symulujący pojedynek 1v1 pomiędzy dwiema klasami postaci - **rycerzem** i **łucznikiem** - walczącymi na zmianę aż do śmierci jednego z nich. Cała logika walki jest wypisywana w konsoli (`Debug.Log`).

## Jak to działa

Scena zawiera trzy obiekty `Cube`, które pełnią funkcję "aktorów" w symulacji:

| Obiekt | Rola | Skrypt |
|---|---|---|
| `Cube` | Zarządca areny | `ArenaManager` |
| `Cube (1)` | Walczący #1 | `knight` |
| `Cube (2)` | Walczący #2 | `archer` |

`ArenaManager` w metodzie `Start()` uruchamia korutynę `StartDuel()`, która w pętli `while` sprawdza, czy obaj walczący żyją (`isAlive`), i na zmianę wywołuje ich metodę `Attack()`. Po każdym ataku HP obu postaci jest logowane do konsoli, a między atakami następuje 2-sekundowe opóźnienie (`WaitForSeconds(2f)`). Pojedynek kończy się, gdy jedna z postaci osiągnie 0 HP - wtedy wypisywany jest komunikat o zwycięstwie drugiej strony.

## Użyte technologie

- Unity Engine z Universal Render Pipeline (URP) - widoczne po komponentach takich jak UniversalAdditionalCameraData, Global Volume (post-processing URP) oraz ustawieniach renderowania w scenie.
- C# / .NET jako język skryptowy, z wykorzystaniem UnityEngine.MonoBehaviour jako bazy dla logiki gry.
- Unity.Mathematics - zaimportowane w klasie bazowej (static Unity.Mathematics.math), choć w obecnej wersji kodu nieużywane bezpośrednio.
- Coroutines (IEnumerator) - mechanizm Unity do sekwencyjnego, czasowo rozłożonego wykonywania logiki bez blokowania głównego wątku.
- Programowanie obiektowe z dziedziczeniem i polimorfizmem - klasa abstrakcyjna base_class oraz klasy pochodne knight i archer nadpisujące metody wirtualne/abstrakcyjne (Attack, TakeDamage).

## Logika projektu
Projekt implementuje prosty, turowy system walki (turn-based combat) pomiędzy dwiema jednostkami dziedziczącymi po wspólnej klasie bazowej:

Model postaci (base_class) hermetyzuje stan (HP, siła ataku, pancerz) poprzez właściwości z prywatnym/chronionym setterem, zapewniając kontrolę nad modyfikacją danych (np. HP nie może spaść poniżej zera).
Polimorficzna implementacja ataku - każda klasa pochodna definiuje własną strategię ataku i obrony:

- knight zwiększa swoją wartość pancerza przy ataku i redukuje otrzymywane obrażenia o tę wartość (z gwarantowanym minimum 1 punktu obrażeń).
- archer zadaje losowe obrażenia w zadanym zakresie, bez mechanizmu redukcji obrażeń.

Orkiestracja pojedynku (ArenaManager) - oddzielona od logiki samych postaci, odpowiada wyłącznie za sterowanie przebiegiem starcia: naprzemienne wywołania ataków, opóźnienia czasowe między turami oraz warunek zakończenia pętli (śmierć jednej ze stron).
Cykl życia obiektu - gdy HP jednostki osiąga zero, obiekt jest automatycznie usuwany ze sceny (Destroy(gameObject)), co synchronizuje stan logiczny z reprezentacją w hierarchii Unity.

To rozdzielenie odpowiedzialności - dane i zachowanie postaci w klasach dziedziczących, a przebieg rozgrywki w osobnym managerze — jest przykładem podstawowego wzorca separacji logiki domenowej od logiki sterującej (control flow), co ułatwia rozbudowę o kolejne typy jednostek bez modyfikacji ArenaManager.

## Struktura klas

- **`base_class`** (abstrakcyjna, dziedziczy po `MonoBehaviour`) - bazowa klasa dla wszystkich walczących. Definiuje:
  - `CurrentHP`, `AttackPower`, `ArmorClass` jako właściwości (enkapsulacja pól `_CurrentHP`, `_AttackPower`, `_ArmorClass`)
  - `isAlive` - zwraca `true`, dopóki HP > 0
  - `TakeDamage(int)` - wirtualna metoda odejmująca obrażenia i niszcząca obiekt (`Destroy(gameObject)`) po spadku HP do 0
  - `Attack(base_class target)` - metoda abstrakcyjna, implementowana przez klasy pochodne

- **`knight` : base_class** - wojownik z pancerzem. `Attack()` ustawia `ArmorClass = 2` i zadaje obrażenia celowi. Nadpisane `TakeDamage()` redukuje przychodzące obrażenia o wartość pancerza (minimum 1 punkt obrażeń zawsze przechodzi).

- **`archer` : base_class** - łucznik o zmiennej sile ataku. `Attack()` losuje wartość obrażeń z przedziału 5-15 (`Random.Range(5, 15)`) i zadaje je celowi. `TakeDamage()` korzysta z domyślnej implementacji bazowej (brak redukcji obrażeń).

- **`ArenaManager` : MonoBehaviour** - łączy dwóch walczących (pola `Fighter1`, `Fighter2` ustawiane w Inspektorze) i steruje przebiegiem pojedynku poprzez korutynę.

## Zawartość sceny

Scena Unity (`.unity`) zawiera standardowe obiekty konfiguracyjne (Occlusion Culling, Render Settings, Lightmap Settings, NavMesh Settings) oraz:
- **Main Camera** - kamera z komponentem `UniversalAdditionalCameraData` (URP)
- **Directional Light** - oświetlenie kierunkowe z cieniami
- **Global Volume** - wolumen post-processingu URP
- Trzy obiekty `Cube` opisane wyżej, każdy z `BoxCollider`, `MeshRenderer` i `MeshFilter`

## Wymagania

- Unity (wersja korzystająca z **Universal Render Pipeline / URP**)
- .NET / C# (skrypty korzystają m.in. z `Unity.Mathematics`)

## Uruchomienie

1. Otwórz scenę w Unity.
2. Upewnij się, że w komponencie `ArenaManager` na obiekcie `Cube` przypisane są pola `Fighter1` (obiekt z komponentem `knight`) i `Fighter2` (obiekt z komponentem `archer`).
3. Wejdź w tryb Play - przebieg pojedynku będzie widoczny w oknie Console.

## Znane problemy

- Komunikaty w konsoli zawierają polskie znaki diakrytyczne, które renderują się niepoprawnie (`zwyciê¿a` zamiast `zwycięża`) - prawdopodobnie skrypty zapisane są w niewłaściwym kodowaniu (nie UTF-8). Do naprawienia poprzez zapisanie plików `.cs` w kodowaniu UTF-8 (bez BOM lub z BOM, w zależności od konfiguracji edytora).
- Brak obsługi remisu/edge case'ów (np. jednoczesna śmierć obu postaci w tej samej klatce nie jest jednoznacznie obsłużona w logice zwycięstwa).
