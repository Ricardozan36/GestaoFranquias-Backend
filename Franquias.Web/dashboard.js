document.addEventListener('DOMContentLoaded', async function() {
    
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
            
            let totalFaturamento = 0;
            vendas.forEach(venda => {
                totalFaturamento += (venda.valorTotal || venda.ValorTotal || 0);
            });
            document.getElementById('valor-faturamento').innerText = totalFaturamento.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

            const tabelaVendas = document.getElementById('tabela-vendas');
            tabelaVendas.innerHTML = ''; 

            const ultimasVendas = vendas.slice(-5).reverse();

            if (ultimasVendas.length === 0) {
                // Ajustado para ocupar 4 colunas agora (colspan="4")
                tabelaVendas.innerHTML = '<tr><td colspan="4" style="text-align: center;">Nenhuma venda registrada ainda.</td></tr>';
            } else {
                ultimasVendas.forEach(venda => {
                    const tr = document.createElement('tr');
                    
                    const id = venda.id || venda.Id || '-';
                    const valorFormatado = (venda.valorTotal || venda.ValorTotal || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
                    
                    let dataFormatada = "-";
                    if (venda.dataVenda || venda.DataVenda) {
                        dataFormatada = new Date(venda.dataVenda || venda.DataVenda).toLocaleDateString('pt-BR');
                    }

                    // ==========================================
                    // NOVA LÓGICA INTELIGENTE DOS PRODUTOS
                    // ==========================================
                    let descricaoProdutos = "Itens da venda";
                    const listaItens = venda.itens || venda.Itens;

                    if (listaItens && listaItens.length > 0) {
                        const nomes = listaItens.map(item => {
                            // Tenta pegar o nome do produto se o C# tiver enviado o objeto aninhado
                            const prod = item.produto || item.Produto;
                            if (prod && (prod.nome || prod.Nome)) {
                                return prod.nome || prod.Nome;
                            }
                            // Fallback caso o C# mande só os IDs e as quantidades
                            const qtd = item.quantidade || item.Quantidade || 1;
                            return `${qtd}x Item(s)`; 
                        });
                        // Junta os nomes com vírgula
                        descricaoProdutos = nomes.join(', ');
                    }

                    tr.innerHTML = `
                        <td>#${id}</td>
                        <td style="font-weight: 500; color: #3f4254;">${descricaoProdutos}</td> <!-- NOVA CÉLULA AQUI -->
                        <td>${dataFormatada}</td>
                        <td class="text-success" style="font-weight: 500;">${valorFormatado}</td>
                    `;
                    tabelaVendas.appendChild(tr);
                });
            }
        } else {
            document.getElementById('valor-faturamento').innerText = "R$ 0,00";
            document.getElementById('tabela-vendas').innerHTML = '<tr><td colspan="4" style="text-align: center;">Erro ao carregar vendas.</td></tr>';
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

    // Botão de Sair 
    document.getElementById('btn-sair').addEventListener('click', function(e) {
        e.preventDefault();
        window.location.href = 'pdv.html';
    });
});