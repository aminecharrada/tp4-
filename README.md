# <p style="text-align:center">TP 4 : Les Tableaux et Les Collections en C# .NET </p> 

---

## 📋 Objectifs du TP  
1. Comprendre les limites des tableaux statiques.  
2. Découvrir les avantages des collections génériques (`List<T>`).  
3. Comparer l'utilisation d'une classe métier avec un dictionnaire.  
4. Manipuler le controle `DataGridView`

---

## 🚀 Étape 0 : Projet de Démarrage  
1- Téléchargez ce projet depuis `Azure DevOps` :  
```bash  
git clone https://TutorialSAID@dev.azure.com/TutorialSAID/Programmation%20.Net%20CSharp/_git/TP4  
```
2- Ouvrez le projet dans Visual Studio :

   - Lancez Visual Studio.

   - Sélectionnez Ouvrir un projet.

   - Naviguez jusqu'au dossier extrait et ouvrez le fichier TP4.sln.
- Ce dépôt contient un projet Windows Forms préconfiguré avec :

    - Un formulaire GestionEtudiantsForm.

    - Les contrôles suivants déjà ajoutés :

       - `DataGridView` (nom : `dataGridView`).

       - `TextBox` pour le nom (`textBoxNom`) et un autre pour la recherche (`textBoxRecherche`).
	   - `NumericUpDown` l’âge (`numericUpDownAge`).

       - Boutons : "Ajouter" (`buttonAjouter`), "Modifier" (`buttonModifier`), "Supprimer" (`buttonSupprimer`),"Rechercher" (`buttonRechercher`) et Trier par Nom" (`buttonTrier`).  

---

## 📝 Activité 1 : Tableaux Statiques de Dictionnaires  
### Classe `GestionEtudiantsForm` (Formulaire Principal)  
```csharp  
public partial class GestionEtudiantsForm : Form  
{  
    // Tableau FIXE de dictionnaires  
    private Dictionary<string, object>[] _etudiantsTableau = new Dictionary<string, object>[10];  
    private int _prochainId = 1;  

    public GestionEtudiantsForm()  
    {  
        InitializeComponent(); 
        ConfigurerDataGridView();
        InitialiserDonneesTest();  
        RafraichirGrille();  
    }  

    // --- Méthodes d'initialisation ---  

    private void ConfigurerDataGridView()
    {
        // Création manuelle des colonnes
        dataGridView.Columns.Add("Id", "ID");
        dataGridView.Columns.Add("Nom", "Nom");
        dataGridView.Columns.Add("Age", "Âge");
    }

    private void InitialiserDonneesTest()  
    {  
        AjouterEtudiant("Alice", 20);  
        AjouterEtudiant("Bob", 22);  
    }  

    private void RafraichirGrille()  
    {  
        dataGridView.Rows.Clear();  
        foreach (var etudiant in _etudiantsTableau)  
        {  
            if (etudiant != null)  
            {  
                dataGridView.Rows.Add(etudiant["Id"], etudiant["Nom"], etudiant["Age"]);  
            }  
        }  
    }  

    // --- Opérations CRUD ---  
    private void AjouterEtudiant(string nom, int age)  
    {  
        if (_prochainId - 1 < _etudiantsTableau.Length)  
        {  
            _etudiantsTableau[_prochainId - 1] = new Dictionary<string, object>  
            {  
                { "Id", _prochainId },  
                { "Nom", nom },  
                { "Age", age }  
            };  
            _prochainId++;  
        }  
        else  
        {  
            MessageBox.Show("Tableau plein !");  
        }  
    }  

    private void ModifierEtudiant(int id, string nom, int age)  
    {  
        for (int i = 0; i < _etudiantsTableau.Length; i++)  
        {  
            if (_etudiantsTableau[i] != null && (int)_etudiantsTableau[i]["Id"] == id)  
            {  
                _etudiantsTableau[i]["Nom"] = nom;  
                _etudiantsTableau[i]["Age"] = age;  
                break;  
            }  
        }  
    }  

    private void SupprimerEtudiant(int id)  
    {  
        for (int i = 0; i < _etudiantsTableau.Length; i++)  
        {  
            if (_etudiantsTableau[i] != null && (int)_etudiantsTableau[i]["Id"] == id)  
            {  
                _etudiantsTableau[i] = null;  
                break;  
            }  
        }  
    }  

    // --- Tri et Recherche ---  
    private void TrierParNom()  
    {  
        for (int i = 0; i < _etudiantsTableau.Length - 1; i++)  
        {  
            for (int j = 0; j < _etudiantsTableau.Length - i - 1; j++)  
            {  
                if (_etudiantsTableau[j] != null && _etudiantsTableau[j + 1] != null &&  
                    String.Compare((string)_etudiantsTableau[j]["Nom"], (string)_etudiantsTableau[j + 1]["Nom"]) > 0)  
                {  
                    var temp = _etudiantsTableau[j];  
                    _etudiantsTableau[j] = _etudiantsTableau[j + 1];  
                    _etudiantsTableau[j + 1] = temp;  
                }  
            }  
        }  
    }  

    private void RechercherParNom(string nom)  
    {  
        foreach (var etudiant in _etudiantsTableau)  
        {  
            if (etudiant != null && etudiant["Nom"].ToString().ToLower().Contains(nom.ToLower()))  
            {  
                MessageBox.Show($"Trouvé : {etudiant["Nom"]} (ID: {etudiant["Id"]})");  
                return;  
            }  
        }  
        MessageBox.Show("Aucun résultat.");  
    }  

    // --- Gestionnaires d'événements ---  
    private void buttonAjouter_Click(object sender, EventArgs e)  
    {  
        if (!string.IsNullOrEmpty(textBoxNom.Text) && int.TryParse(numericUpDownAge.Value.ToString(), out int age))  
        {  
            AjouterEtudiant(textBoxNom.Text, age);  
            RafraichirGrille();  
        }  
    }  

    private void buttonModifier_Click(object sender, EventArgs e)  
    {  
        if (dataGridView.SelectedRows.Count > 0)  
        {  
            int id = (int)dataGridView.SelectedRows[0].Cells[0].Value;  
            ModifierEtudiant(id, textBoxNom.Text, int.Parse(numericUpDownAge.Value.ToString()));  
            RafraichirGrille();  
        }  
    }  

    private void buttonSupprimer_Click(object sender, EventArgs e)  
    {  
        if (dataGridView.SelectedRows.Count > 0)  
        {  
            int id = (int)dataGridView.SelectedRows[0].Cells[0].Value;  
            SupprimerEtudiant(id);  
            RafraichirGrille();  
        }  
    }  

    private void buttonTrier_Click(object sender, EventArgs e)  
    {  
        TrierParNom();  
        RafraichirGrille();  
    }  

    private void buttonRechercher_Click(object sender, EventArgs e)  
    {  
        RechercherParNom(textBoxRecherche.Text);  
    }  
}  
```  

### 🔍 Questions  
1. **Limites des tableaux** :  
   - Que se passe-t-il si vous essayez d’ajouter un 11ᵉ étudiant dans le tableau de taille 10 ?  
   - Comment pourriez-vous gérer les "trous" laissés par les suppressions dans le tableau ?
2. **Transition** :  
   - Comment les collections génériques pourraient-elles résoudre les problèmes identifiés ici ? 
---

## 📝 Activité 2 : Collections avec `List<Dictionary>`  
### Classe `GestionEtudiantsForm` (Modifiée)  
```csharp  
public partial class GestionEtudiantsForm : Form  
{  
    private List<Dictionary<string, object>> _etudiantsListe = new List<Dictionary<string, object>>();  
    private int _prochainId = 1;  

    // --- Méthodes CRUD ---  
    private void AjouterEtudiant(string nom, int age)  
    {  
        _etudiantsListe.Add(new Dictionary<string, object>  
        {  
            { "Id", _prochainId },  
            { "Nom", nom },  
            { "Age", age }  
        });  
        _prochainId++;  
    }  

    private void ModifierEtudiant(int id, string nom, int age)  
    {  
        foreach (var etudiant in _etudiantsListe)  
        {  
            if ((int)etudiant["Id"] == id)  
            {  
                etudiant["Nom"] = nom;  
                etudiant["Age"] = age;  
                break;  
            }  
        }  
    }  

    private void SupprimerEtudiant(int id)  
    {  
        for (int i = 0; i < _etudiantsListe.Count; i++)  
        {  
            if ((int)_etudiantsListe[i]["Id"] == id)  
            {  
                _etudiantsListe.RemoveAt(i);  
                break;  
            }  
        }  
    }  

    // --- Tri et Recherche ---  
    private void TrierParNom()  
    {  
        for (int i = 0; i < _etudiantsListe.Count - 1; i++)  
        {  
            for (int j = 0; j < _etudiantsListe.Count - i - 1; j++)  
            {  
                if (String.Compare((string)_etudiantsListe[j]["Nom"], (string)_etudiantsListe[j + 1]["Nom"]) > 0)  
                {  
                    var temp = _etudiantsListe[j];  
                    _etudiantsListe[j] = _etudiantsListe[j + 1];  
                    _etudiantsListe[j + 1] = temp;  
                }  
            }  
        }  
    }  

    // (Les autres méthodes restent similaires à l'Activité 1)  
}  
```  

### 🔍 Questions  
1. **Avantages des collections** :  
   - Pourquoi la suppression est-elle plus simple avec une `List<T>` qu’avec un tableau statique ?  
   - Comment `List<T>` gère-t-elle dynamiquement la mémoire ? 
2. **Réflexion** :  
   - Quels inconvénients voyez-vous à utiliser des dictionnaires pour représenter des entités métier ? 
---

## 📝 Activité 3 : Changer `Dictionary<TKey, TValue>` par une `class`   
### Classe `Etudiant`  
```csharp  
public class Etudiant  
{  
    public int Id { get; }  
    public string Nom { get; set; }  
    public int Age { get; set; }  
    private static int _idCourant = 1;  

    public Etudiant(string nom, int age)  
    {  
        Id = _idCourant++;  
        Nom = nom;  
        Age = age;  
    }  
}  
```  

### Classe `GestionEtudiantsForm` (Version POO)  
```csharp  
public partial class GestionEtudiantsForm : Form  
{  
    private List<Etudiant> _etudiants = new List<Etudiant>();
    
    // --- Méthodes d'initialisation ---  on change seullement la méthode RafraichirGrille
    private void RafraichirGrille()
    {
        dataGridView.Rows.Clear();
        foreach (var etudiant in _etudiants)
        {
            if (etudiant != null)
            {
                dataGridView.Rows.Add(etudiant.Id, etudiant.Nom, etudiant.Age);
            }
        }
    }

    // --- Méthodes CRUD ---  
    private void AjouterEtudiant(string nom, int age)  
    {  
        _etudiants.Add(new Etudiant(nom, age));  
    }  

    private void ModifierEtudiant(int id, string nom, int age)  
    {  
        Etudiant etudiant = _etudiants.FirstOrDefault(e => e.Id == id);  
        if (etudiant != null)  
        {  
            etudiant.Nom = nom;  
            etudiant.Age = age;  
        }  
    }  

    private void SupprimerEtudiant(int id)  
    {  
        Etudiant etudiant = _etudiants.FirstOrDefault(e => e.Id == id);  
        if (etudiant != null)  
        {  
            _etudiants.Remove(etudiant);  
        }  
    }  

    // --- Tri et Recherche ---  
    private void TrierParNom()  
    {  
        for (int i = 0; i < _etudiants.Count - 1; i++)  
        {  
            for (int j = 0; j < _etudiants.Count - i - 1; j++)  
            {  
                if (String.Compare(_etudiants[j].Nom, _etudiants[j + 1].Nom) > 0)  
                {  
                    Etudiant temp = _etudiants[j];  
                    _etudiants[j] = _etudiants[j + 1];  
                    _etudiants[j + 1] = temp;  
                }  
            }  
        }  
    }  

    private void RechercherParNom(string nom)  
    {  
        foreach (Etudiant etudiant in _etudiants)  
        {  
            if (etudiant.Nom.ToLower().Contains(nom.ToLower()))  
            {  
                MessageBox.Show($"Trouvé : {etudiant.Nom} (ID: {etudiant.Id})");  
                return;  
            }  
        }  
        MessageBox.Show("Aucun résultat.");  
    }  

    // (Les gestionnaires d'événements restent similaires)  
}  
```  

### 🔍 Questions  
1. **Encapsulation** :  
   - Pourquoi la propriété `Id` est-elle en lecture seule (`get` uniquement) ?  
   - Comment la classe `Etudiant` empêche-t-elle les incohérences de données ?  

2. **Maintenabilité** :  
   - Si vous ajoutez une propriété `Email` à la classe `Etudiant`, quelles parties du code devez-vous modifier ?  
   - Comparez cette maintenance avec celle nécessaire si vous utilisiez des dictionnaires.  

3. **Design** :   
   - Comment pourriez-vous implémenter une validation d’âge directement dans la classe `Etudiant` ?
--- 

## 🧠 Questions Finales de Synthèse  
1. **POO vs Dictionnaires** :  
   - Dans quel scénario préféreriez-vous utiliser un dictionnaire plutôt qu’une classe ?  

2. **Collections génériques** :  
   - Pourquoi `List<T>` est-elle considérée comme une collection "de haut niveau" par rapport aux tableaux ?  

3. **Évolutivité** :  
   - Comment adapteriez-vous ce projet pour gérer 100 000 étudiants sans perte de performance ?  

--- 

## 📤 Livrables  
1. Code source complet des 3 activités.
2. Réponses aux questions pour chaque activité
3. Rapport comparatif (tableau) des avantages/inconvénients de chaque approche.   

---

## 🔗 Ressources  
- [Documentation `DataGridView`](https://learn.microsoft.com/fr-fr/dotnet/api/system.windows.forms.datagridview?view=windowsdesktop-8.0)
- [Documentation `Array` (Tableau)](https://learn.microsoft.com/fr-fr/dotnet/api/system.array?view=net-8.0)
- [Documentation `Dictionary<TKey,TValue>`](https://learn.microsoft.com/fr-fr/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0)
- [Documentation `List<T>`](https://learn.microsoft.com/fr-fr/dotnet/api/system.collections.generic.list-1?view=net-8.0)  
- [Azure DevOps du projet](https://dev.azure.com/TutorialSAID/Programmation%20.Net%20CSharp/_git/TP4)  

---
**Bon codage !** 😊
<div style="border: 2px solid #2196F3; padding: 15px; border-radius: 10px; background: #E3F2FD;">
  <div style="display: flex; align-items: center; gap: 10px;">
    <span style="font-size: 24px;">🚀</span>
    <div>
      <h3 style="margin: 0; color: #0D47A1;">Alerte Développeur !</h3>
      <p style="margin: 5px 0;">
        <strong>Objet :</strong> Mission Collections C#<br>
        <strong>Niveau :</strong> Expert en devenir<br>
        <strong>Récompense :</strong> Maîtrise des DataGridView ✨
      </p>
      <div style="background: #BBDEFB; padding: 8px; border-radius: 5px;">
        <code style="color: #1976D2;">$ git commit -m "Je vais tout déchirer sur ce TP !"</code>
      </div>
    </div>
  </div>
</div>
