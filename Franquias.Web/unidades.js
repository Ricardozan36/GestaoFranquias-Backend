document.addEventListener('DOMContentLoaded', async function() {
    
    // 1. Validação de segurança (Token)
    const token = localStorage.getItem('authToken');
    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    const fetchOptions = {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/json'
        }
    };

    try {
        // 2. Busca os dados no Back-End
        const resUnidades = await fetch('http://localhost:5062/api/unidades', fetchOptions);
        const tabela = document.getElementById('tabela-unidades');

        if (resUnidades.ok) {
            const unidades = await resUnidades.json();
            tabela.innerHTML = ''; // Limpa a mensagem de carregando

            if (unidades.length === 0) {
                tabela.innerHTML = '<tr><td colspan="3" style="text-align: center;">Nenhuma unidade cadastrada.</td></tr>';
            } else {
                // 3. Desenha as linhas da tabela
                unidades.forEach(unidade => {
                    const tr = document.createElement('tr');
                    
                    // Pegamos os campos adaptando para maiúsculo ou minúsculo conforme o C# devolve
                    const id = unidade.id || unidade.Id || '-';
                    const nome = unidade.nome || unidade.Nome || 'Unidade sem nome';
                    
                    // Se o status for 1 (Ativa), pinta de verde. Se não, pinta de vermelho.
                    const statusCodigo = unidade.status || unidade.Status;
                    let statusHtml = '';
                    if (statusCodigo === 1 || statusCodigo === 'Ativa') {
                        statusHtml = '<span class="text-success" style="font-weight: 500;">Ativa</span>';
                    } else {
                        statusHtml = '<span class="text-danger" style="font-weight: 500;">Inativa</span>';
                    }

                    tr.innerHTML = `
                        <td>#${id}</td>
                        <td>${nome}</td>
                        <td>${statusHtml}</td>
                    `;
                    tabela.appendChild(tr);
                });
            }
        } else {
            tabela.innerHTML = '<tr><td colspan="3" style="text-align: center;">Erro ao carregar unidades.</td></tr>';
        }

    } catch (error) {
        console.error("Erro de conexão: ", error);
        document.getElementById('tabela-unidades').innerHTML = '<tr><td colspan="3" style="text-align: center;">Erro de conexão com a API.</td></tr>';
    }

    // Configura o botão de sair
    document.getElementById('btn-sair').addEventListener('click', function() {
        localStorage.removeItem('authToken');
        window.location.href = 'index.html';
    });
});