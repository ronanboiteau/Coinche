# Coinche

This is a .NET online Coinche game. It requires 4 players to play. You can find the rules of this game on [Wikipedia](https://en.wikipedia.org/wiki/Coinche).

## Getting Started

### Prerequisites

To build and run the project you need to install:
* [.NET Core](https://www.microsoft.com/net/download/)

## How to run the server & clients

### Running the server

To run the Coinche server, use your terminal to navigate to the `Server/` folder located at the root of the repository & then run:
```
dotnet run
```

### Running clients

To run Coinche clients, use your terminal to navigate to the `Client/` folder located at the root of the repository & then run:
```
dotnet run
```
You need to run **4 clients** to start a Coinche game.

## How to play

Once you've run the 4 clients, the game starts. You are now a player in a team of 2.

#### Bidding

You are given 8 cards with different IDs.
ex.:
```
Here is your card deck:
| (18) 9 HEARTS | (2) 9 DIAMONDS | (21) Q HEARTS | (15) A CLUBS | (6) K DIAMONDS |
```
For the game to start, a player needs to make the highest bid (https://en.wikipedia.org/wiki/Coinche#Bidding).
To make a bid you need to specify the value of your bid *(80, 90, 100, 110, 120, 130, 140, 150 or 160)* and a card suit *(HEARTS, DIAMONDS, CLUBS or SPADES)*.
Bidding ex.:
```
80 SPADES
```
To pass, you just need to type
```
PASS
```
If every player passes, the cards are re-drawn and the bidding will start again.
The bidding will stop when only one player bids and the other ones pass.

#### Game

The player who made the highest bid starts. He needs to type the ID *(shown in the deck between parentheses)* of the card he wants to play.
```
18
```
After that, every player has to play. The one who wins the trick will increase the score of his team.

Once the players are out of cards, the game ends. 
If the team who made the bid has at least the points of their contract and has more points than the other team, they win, and the other team loses.

## Authors

* **Ronan Boiteau** ([GitHub](https://github.com/ronanboiteau) / [LinkedIn](https://www.linkedin.com/in/ronanboiteau/))
* **Fanny Tavart**  ([GitHub](https://github.com/fannytavart) / [LinkedIn](https://www.linkedin.com/in/fannytavart/))
