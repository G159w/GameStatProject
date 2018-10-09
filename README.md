# ⚠ Dev environnement setup

## 1 - appsettings.json

Modify the `Default` connection string according to your local environnment. 

## 2 - Database synchronisation

This is a database first project, you have to sync your database with the committed SQL schema using the `Database` project of the solution. You can do this by following these steps :

1) Right-click on the `Database` project and click on `Schema compare`
2) Select the source (left dropdown) and the target (right dropdown)
3) Click on compare and wait until the comparison has ended
4) Click on update to update the target according to the comparison

## 3 - T_Game table

The T_Game table is the one holding the names/list of the supported games.
**Use the `Scripts/DatabaseInit.sql` script to init this table.**

# 🎮 Examples games usernames

## Elite : Dangerous

- `Space Koala`
- `Disorganise`

## Rainbow Six : Siege

- `Space.Koala`
- `Yuxi_`

## League of Legends

- `Hugh Jukeman`
- `Miracomotepeto`

## Fortnite

- `Exuhz`
- `PierXBL`
