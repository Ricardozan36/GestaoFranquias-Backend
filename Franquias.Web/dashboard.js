document.addEventListener('DOMContentLoaded', async function() {
    
    // 1. Pega a chave do cofre
    const token = localStorage.getItem('authToken');

    // Se não tiver token, expulsa de volta pro Login
    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    // Configuração padrão para as requisições
    const fetchOptions = {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/json'
        }
    };

    try {
        // --- BUSCA UNIDADES ---
        const resUnidades = await fetch('http://localhost:5062/api/unidades', fetchOptions);
        if (resUnidades.ok) {
            const unidades = await resUnidades.json();
            document.getElementById('qtd-unidades').innerText = unidades.length;
        }

        // --- BUSCA FATURAMENTO (Vendas) ---
        const resVendas = await fetch('http://localhost:5062/api/vendas', fetchOptions);
        if (resVendas.ok) {
            const vendas = await resVendas.json();
            
            // Soma o valor de todas as vendas (Assumindo que sua classe C# tem uma propriedade 'valor' ou 'Valor')
            let totalFaturamento = 0;
            vendas.forEach(venda => {
                totalFaturamento += (venda.valor || venda.Valor || 0);
            });

            // Formata bonitinho para Reais (R$)
            document.getElementById('valor-faturamento').innerText = totalFaturamento.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        } else {
            document.getElementById('valor-faturamento').innerText = "R$ 0,00";
        }

        // --- BUSCA CHAMADOS ---
        const resChamados = await fetch('http://localhost:5062/api/chamados', fetchOptions);
        if (resChamados.ok) {
            const chamados = await resChamados.json();
            document.getElementById('qtd-chamados').innerText = chamados.length;
        } else {
            document.getElementById('qtd-chamados').innerText = "0";
        }

    } catch (error) {
        console.error("Erro de conexão com a API: ", error);
        document.getElementById('qtd-unidades').innerText = "Erro";
        document.getElementById('valor-faturamento').innerText = "Erro";
        document.getElementById('qtd-chamados').innerText = "Erro";
    }

    // Configura o botão de sair
    document.getElementById('btn-sair').addEventListener('click', function() {
        localStorage.removeItem('authToken');
        window.location.href = 'index.html';
    });
});