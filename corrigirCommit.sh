#!/bin/bash
# ===== CONFIG =====
COMMIT_ID="$1"        # Exemplo: e7b6b4e
NOVO_NOME="$2"        # Exemplo: "João Silva"
NOVO_EMAIL="$3"       # Exemplo: "joao@gmail.com"
BRANCH_ATUAL="TelaInicial-Lucas-Pedro"

# ===== VALIDAÇÃO =====
if [ -z "$COMMIT_ID" ] || [ -z "$NOVO_NOME" ] || [ -z "$NOVO_EMAIL" ]; then
  echo "Uso: ./corrigir_autor.sh <commit_id> \"Nome do Autor\" email@exemplo.com"
  exit 1
fi

echo "🔧 Alterando autor do commit $COMMIT_ID..."
echo "   Nome:  $NOVO_NOME"
echo "   E-mail: $NOVO_EMAIL"
echo

# ===== PROCESSO =====
git rebase -i ${COMMIT_ID}^ --autosquash --autostash --no-autosquash --quiet &
PID=$!
wait $PID

GIT_SEQUENCE_EDITOR="sed -i 's/^pick $COMMIT_ID/edit $COMMIT_ID/'" git rebase -i ${COMMIT_ID}^
git commit --amend --author="$NOVO_NOME <$NOVO_EMAIL>" --no-edit
git rebase --continue

# ===== FINAL =====
git push --force origin $BRANCH_ATUAL
echo
echo "✅ Autor do commit $COMMIT_ID alterado com sucesso e enviado pro remoto!"
