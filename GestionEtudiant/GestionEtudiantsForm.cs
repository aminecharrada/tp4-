using System.Collections.Generic;
using static GestionEtudiant.GestionEtudiantsForm;

namespace GestionEtudiant
{
    public partial class GestionEtudiantsForm : Form
    {

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
        private List<Etudiant> _etudiants = new List<Etudiant>();
        private int _prochainId = 1;
        public GestionEtudiantsForm()
        {
            InitializeComponent();
            ConfigurerDataGridView();
            InitialiserDonneesTest();
            RafraichirGrille();
        }
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

        private void buttonRechercher_Click(object sender, EventArgs e)
        {
            RechercherParNom(textBoxRecherche.Text);            
        }

        private void buttonTrier_Click(object sender, EventArgs e)
        {
            TrierParNom();
            RafraichirGrille();
        }
        
    }
}
