#!/bin/bash

# Config
SINCE="2026-01-13"
OUTPUT_DIR="Doc"
OUTPUT_FILE="$OUTPUT_DIR/journal_de_travail.csv"

mkdir -p "$OUTPUT_DIR"

echo "🚀 Scan des commits avec colonnes Temps et État..."

# En-tête CSV avec la nouvelle colonne
echo "Nom,Temps,État,Description" > "$OUTPUT_FILE"

# %s = Sujet du commit
git log --all --since="$SINCE" --pretty=format:%s%x1e \
| awk -v RS='\036' -F '\037' -v OFS=',' '
NF {
    full_msg = $1;
    temps = "[?]";
    etat = "[?]";
    desc = full_msg;

    # 1. Extraction du Temps (le premier bloc entre crochets)
    if (match(full_msg, /^\[[^\]]+\]/)) {
        temps = substr(full_msg, RSTART, RLENGTH);
        reste = substr(full_msg, RSTART + RLENGTH);
        
        # 2. Extraction de l État (le deuxième bloc entre crochets s il existe)
        if (match(reste, /^\[[^\]]+\]/)) {
            etat = substr(reste, RSTART, RLENGTH);
            desc = substr(reste, RSTART + RLENGTH + 1); # Le reste est la description
        } else {
            desc = substr(reste, 2); # Pas d etat, le reste est la description
        }
    }

    # Nettoyage des guillemets
    gsub(/"/, "\"\"", desc);
    
    # Formatage : Nom vide, puis Temps, État et Description
    printf "\"\",\"%s\",\"%s\",\"%s\"\n", temps, etat, desc;
}
' >> "$OUTPUT_FILE"

echo "✨ C’est dans la boîte ! Ton journal est prêt : $OUTPUT_FILE"