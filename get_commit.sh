#!/bin/bash

# Config
SINCE="2026-01-13"
OUTPUT_DIR="Doc"
OUTPUT_FILE="$OUTPUT_DIR/journal_de_travail.csv"

mkdir -p "$OUTPUT_DIR"

echo "🚀 Extraction chronologique avec séparation par date..."

# En-tête CSV
echo "Date,Nom,Temps,État,Description" > "$OUTPUT_FILE"

# %ad = Date (format YYYY-MM-DD) | %s = Titre | %b = Corps
git log --all --since="$SINCE" --reverse --date=format:'%d/%m/%Y' --pretty=format:%ad%x1f%s%x1f%b%x1e \
| awk -v RS='\036' -F '\037' '
NF {
    current_date = $1;
    nom_commit = $2;
    description_detaillee = $3;
    
    # --- LOGIQUE DE SÉPARATION ---
    # Si la date change par rapport au commit précédent, on saute une ligne
    if (last_date != "" && last_date != current_date) {
        print ",,,,"; # Ligne vide pour séparer visuellement dans Excel
    }
    last_date = current_date;

    gsub(/\n/, " ", description_detaillee);

    temps = "[?]";
    etat = "[?]";

    # Extraction du Temps [1h45min]
    if (match(nom_commit, /\[[0-9hmin]+\]/)) {
        temps = substr(nom_commit, RSTART, RLENGTH);
        sub(/\[[0-9hmin]+\]/, "", nom_commit);
    } else if (match(description_detaillee, /\[[0-9hmin]+\]/)) {
        temps = substr(description_detaillee, RSTART, RLENGTH);
        sub(/\[[0-9hmin]+\]/, "", description_detaillee);
    }

    # Extraction de l État [DONE]
    if (match(nom_commit, /\[[A-Z]+\]/)) {
        etat = substr(nom_commit, RSTART, RLENGTH);
        sub(/\[[A-Z]+\]/, "", nom_commit);
    } else if (match(description_detaillee, /\[[A-Z]+\]/)) {
        etat = substr(description_detaillee, RSTART, RLENGTH);
        sub(/\[[A-Z]+\]/, "", description_detaillee);
    }

    # Nettoyage final
    gsub(/^ +| +$/, "", nom_commit);
    gsub(/^ +| +$/, "", description_detaillee);
    gsub(/"/, "\"\"", nom_commit);
    gsub(/"/, "\"\"", description_detaillee);

    # Affichage avec la Date au début
    printf "\"%s\",\"%s\",\"%s\",\"%s\",\"%s\"\n", current_date, nom_commit, temps, etat, description_detaillee;
}
' >> "$OUTPUT_FILE"

echo "✨ Terminé ! Tes commits sont séparés par jour dans : $OUTPUT_FILE"