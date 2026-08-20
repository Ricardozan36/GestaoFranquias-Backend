// Quando a página carregar, execute essa função
document.addEventListener('DOMContentLoaded', async function() {
    
    // 1. Pega a chave do cofre
    const token = localStorage.getItem('authToken');

    // Se não tiver token, expulsa de volta pro Login
    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    try {
        // 2. Bate na porta da API com o Token no cabeçalho
        const response = await fetch('http://localhost:5062/api/unidades', {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            // 3. Converte a resposta e conta as unidades
            const unidades = await response.json();
            
            // 4. Injeta o número real no HTML!
            document.getElementById('qtd-unidades').innerText = unidades.length;
        } else {
            console.error("Erro ao buscar unidades. Token pode estar vencido.");
        }
    } catch (error) {
        console.error("Erro de conexão com a API: ", error);
        document.getElementById('qtd-unidades').innerText = "Erro";
    }

    // Configura o botão de sair
    document.getElementById('btn-sair').addEventListener('click', function() {
        localStorage.removeItem('authToken');
        window.location.href = 'index.html';
    });
});