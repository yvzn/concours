# Grand prix de Monaco

Pour lancer à partir d'un exemple:

```powershell
dotnet build
Get-Content .\samples\input2.txt | .\bin\Debug\netcoreapp3.1\grand-prix-de-monaco.exe
```

Pour lancer à partir de STDIN:

```powershell
.\bin\Debug\netcoreapp3.1\grand-prix-de-monaco.exe
...DATA...
...DATA...
...DATA...
```

Puis <kbd>CTRL</kbd> + <kbd>Z</kbd> pour arrêter la saisie et afficher le résultat
