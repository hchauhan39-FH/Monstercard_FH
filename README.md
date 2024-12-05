# Monstercard_FH - Chauhan

## Project Overview

The goal is to implement a REST-based server in Java or C# for the **Monster Trading Cards Game (MCTG)**. The server will act as an API for various frontends, such as WPF, JavaFX, Web, or console interfaces. This protocol covers the server-side logic, gameplay mechanics, trading system, and API endpoints.

---

## Key Features

### 1. **User Management**
- **Registration**: Users must register with a unique username and password.
- **Login**: Users can log in using their credentials, receiving a token for authentication.
- **Card Management**: Users can manage cards (add/remove cards to/from their stack).
- **Deck Creation**: Users select 4 cards to form their deck, which will be used in battles.

### 2. **Card Details**
- **Card Types**: 
  - **Monster-Cards**: Have an element type (fire, water, normal) and damage.
  - **Spell-Cards**: Also have an element type and affect damage based on the opponent’s card.
- **Damage**: Damage is constant and does not change during the game.

### 3. **Gameplay**
- **Stack**: A collection of all cards owned by the user.
- **Buying Cards**: Users can purchase card packages (5 cards for 5 coins). Each user starts with 20 coins.
- **Battle**: Users battle other players using their deck. The battle is turn-based, where randomly selected cards from each deck compete.

### 4. **Battle Logic**
- **Card Categories**: 
  - **Monster-cards**: Active attacks with elemental damage.
  - **Spell-cards**: Spell-based attacks where element types can enhance or weaken damage.
- **Damage Calculation**: Element types affect damage (e.g., water > fire).
- **Battle Rounds**: Each round, both players randomly select one card to fight. The card with higher damage wins. The game ends after 100 rounds or when one player runs out of cards.
- **Special Rules**: Specific interactions between cards (e.g., Goblins fear Dragons, Wizards control Orks, etc.).

---

## Trading System
- **Trade Requests**: Users can put cards into the trade store, with specific requirements for the type or damage of cards they wish to trade for.
- **Trade Example**:  
  - Player A offers a WaterGoblin (50 damage) and requests a spell card with at least 70 damage.
  - Player B offers a RegularSpell (80 damage) in exchange.

---

## Additional Features
- **Scoreboard**: Users can view their rank, based on their ELO score.
- **Profile Page**: Users can edit their profile.
- **ELO System**: 
  - Win: +3 points
  - Loss: -5 points
  - Starting ELO: 100

- **Security**: Use of authentication tokens to ensure users can only perform actions on their own account.

---

## Implementation Requirements

### 1. **Server Setup**
- **REST API**: Implement the HTTP server and API without using external HTTP frameworks.
- **Serialization**: Allowed to use libraries (e.g., Jackson) for object serialization.
- **Database**: Use PostgreSQL for data persistence.

### 2. **Testing**
- **Unit Tests**: Implement at least 20 unit tests to verify application logic.
- **Integration Test**: Test using the provided `curl` script for API interaction.

---

## HTTP API Specification

### Example Request (Login)

```bash
curl -X POST http://localhost:10001/sessions \
--header "Content-Type: application/json" \
-d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
