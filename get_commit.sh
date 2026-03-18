#!/bin/bash
 
# Config
SINCE="2026-01-13"
OUTPUT_DIR="Doc"
OUTPUT_FILE="$OUTPUT_DIR/journal_de_travail.csv"
 
mkdir -p "$OUTPUT_DIR"
 
echo "🚀 Extraction propre en cours..."
 
# 1. On écrit l'en-tête (écrase le fichier existant)
echo "Date,Nom,Temps,État,Description" > "$OUTPUT_FILE"
 
# 2. Git log avec séparateurs spéciaux
git log --all --since="$SINCE" --reverse --date=format:'%d/%m/%Y' --pretty=format:%ad%x1f%s%x1f%b%x1e \
| awk -v RS='\036' -F '\037' '
NF {
    current_date = $1;
    nom_commit = $2;
    body = $3;
 
    # Le séparateur de records peut laisser un saut de ligne en tête
    gsub(/\r|\n/, "", current_date);
    gsub(/^ +| +$/, "", current_date);
 
    # Nettoyage des sauts de ligne pour éviter de casser le CSV
    gsub(/\r|\n/, " ", nom_commit);
    gsub(/\r|\n/, " ", body);
   
    full_text = nom_commit " " body;
 
    # Initialisation
    temps = "[?]";
    etat = "[?]";
 
    # Extraction Temps [1h45min]
    if (match(full_text, /\[[0-9hmin]+\]/)) {
        temps = substr(full_text, RSTART, RLENGTH);
        sub(/\[[0-9hmin]+\]/, "", full_text);
    }
   
    # Extraction État [DONE]
    if (match(full_text, /\[[A-Z]+\]/)) {
        etat = substr(full_text, RSTART, RLENGTH);
        sub(/\[[A-Z]+\]/, "", full_text);
    }
 
    # NETTOYAGE FINAL : on double les guillemets pour le format CSV
    gsub(/"/, "\"\"", nom_commit);
    gsub(/"/, "\"\"", full_text);
    # On enlève les espaces en trop au début et à la fin
    gsub(/^ +| +$/, "", nom_commit);
    gsub(/^ +| +$/, "", full_text);
 
    # SÉPARATION DES JOURS
    if (last_date != "" && last_date != current_date) {
        print "" >> "'$OUTPUT_FILE'";
    }
    last_date = current_date;
 
    # ÉCRITURE DE LA LIGNE (Une seule fois, bien proprement)
    printf "\"%s\",\"%s\",\"%s\",\"%s\",\"%s\"\n", current_date, nom_commit, temps, etat, full_text >> "'$OUTPUT_FILE'";
}
'
 
echo "✨ C'est fini ! Regarde ton fichier : $OUTPUT_FILE"