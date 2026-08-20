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

        // --- BUSCA FATURAMENTO E ÚLTIMAS VENDAS ---
        const resVendas = await fetch('http://localhost:5062/api/vendas', fetchOptions);
        if (resVendas.ok) {
            const vendas = await resVendas.json();
            
            // 1. Soma o valor para o Cartão Superior
            let totalFaturamento = 0;
            vendas.forEach(venda => {
                totalFaturamento += (venda.valor || venda.Valor || 0);
            });
            document.getElementById('valor-faturamento').innerText = totalFaturamento.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

            // 2. Preenche a Tabela Inferior (Pegando as 5 últimas)
            const tabelaVendas = document.getElementById('tabela-vendas');
            tabelaVendas.innerHTML = ''; // Limpa o "Carregando..."

            // Pega os 5 últimos registros e inverte a ordem (para o mais novo ficar no topo)
            const ultimasVendas = vendas.slice(-5).reverse();

            if (ultimasVendas.length === 0) {
                tabelaVendas.innerHTML = '<tr><td colspan="3" style="text-align: center;">Nenhuma venda registrada ainda.</td></tr>';
            } else {
                ultimasVendas.forEach(venda => {
                    const tr = document.createElement('tr');
                    
                    const id = venda.id || venda.Id || '-';
                    const valorFormatado = (venda.valor || venda.Valor || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                    
                    // Se a sua API envia a data da venda, formatamos. Senão, fica um tracinho.
                    let dataFormatada = "-";
                    if (venda.data || venda.Data) {
                        dataFormatada = new Date(venda.data || venda.Data).toLocaleDateString('pt-BR');
                    }

                    tr.innerHTML = `
                        <td>#${id}</td>
                        <td>${dataFormatada}</td>
                        <td class="text-success" style="font-weight: 500;">${valorFormatado}</td>
                    `;
                    tabelaVendas.appendChild(tr);
                });
            }
        } else {
            document.getElementById('valor-faturamento').innerText = "R$ 0,00";
            document.getElementById('tabela-vendas').innerHTML = '<tr><td colspan="3" style="text-align: center;">Erro ao carregar vendas.</td></tr>';
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