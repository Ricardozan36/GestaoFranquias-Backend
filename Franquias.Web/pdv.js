document.addEventListener('DOMContentLoaded', async function() {
    
    // 1. Validação de segurança básica da tela de PDV
    const token = localStorage.getItem('authToken');
    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    const fetchOptions = {
        headers: {
            'Authorization': 'Bearer ' + token,
            'Content-Type': 'application/json'
        }
    };

    // ==========================================
    // LÓGICA DA FRENTE DE CAIXA (CARRINHO E VENDAS)
    // ==========================================
    let carrinho = [];
    let totalVenda = 0;

    // A. Carrega os produtos do banco de dados na inicialização
    try {
        const resProdutos = await fetch('http://localhost:5062/api/produtos', { method: 'GET', ...fetchOptions });
        if (resProdutos.ok) {
            const produtosServidor = await resProdutos.json();
            const selectProduto = document.getElementById('select-produto');
            selectProduto.innerHTML = '<option value="">Selecione um produto...</option>';
            
            // Preenche a caixa de seleção dinamicamente
            produtosServidor.forEach(p => {
                // Adaptação para pegar os nomes caso o C# mande com letra maiúscula ou minúscula
                const id = p.id || p.Id;
                const nome = p.nome || p.Nome;
                const preco = p.precoBase || p.PrecoBase || p.Preco || 0;
                const ativo = p.ativo !== undefined ? p.ativo : p.Ativo;
                
                if(ativo !== false) { // Só mostra se estiver ativo
                    selectProduto.innerHTML += `<option value="${id}" data-preco="${preco}">${nome} - R$ ${preco.toFixed(2)}</option>`;
                }
            });
        } else {
            document.getElementById('select-produto').innerHTML = '<option value="">Erro ao carregar catálogo</option>';
        }
    } catch(err) {
        console.error("Erro ao comunicar com a API:", err);
    }

    // B. Lógica de "Bipar" o produto (Adicionar ao carrinho)
    document.getElementById('btn-adicionar').addEventListener('click', function() {
        const select = document.getElementById('select-produto');
        const qtdInput = document.getElementById('qtd-produto');
        
        const produtoId = select.value;
        const quantidade = parseInt(qtdInput.value);

        if (!produtoId || quantidade < 1) {
            alert("Selecione um produto válido e uma quantidade maior que zero.");
            return;
        }

        // Pega o preço que guardamos no atributo escondido do HTML
        const precoProduto = parseFloat(select.options[select.selectedIndex].getAttribute('data-preco'));
        
        // Coloca o item na memória do carrinho
        carrinho.push({
            produtoServicoId: parseInt(produtoId),
            quantidade: quantidade
        });

        // Atualiza a tela maravilhosa com o novo valor
        totalVenda += (precoProduto * quantidade);
        document.getElementById('total-venda').innerText = totalVenda.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

        // Reseta os inputs para o próximo produto
        select.value = "";
        qtdInput.value = 1;
    });

    // C. Finalizar a Venda no C#
    const btnFinalizar = document.getElementById('btn-finalizar');
    const msgVenda = document.getElementById('msg-venda');

    btnFinalizar.addEventListener('click', async function() {
        if (carrinho.length === 0) {
            msgVenda.innerText = "O carrinho está vazio. Adicione produtos antes de finalizar.";
            msgVenda.style.color = "#f1416c"; // Vermelho erro
            msgVenda.style.display = 'block';
            return;
        }

        btnFinalizar.innerText = "Emitindo Cupom...";

        try {
            // Prepara o pacote idêntico ao NovaVendaDTO do seu C#
            // Nota: Fixamos UnidadeFranqueadaId = 1 para funcionar o teste.
            const payload = {
                unidadeFranqueadaId: 1, 
                itens: carrinho
            };

            const resVenda = await fetch('http://localhost:5062/api/vendas', {
                method: 'POST',
                headers: fetchOptions.headers,
                body: JSON.stringify(payload)
            });

            if (resVenda.ok || resVenda.status === 201) {
                // Sucesso Absoluto!
                msgVenda.innerText = "Venda registrada com sucesso!";
                msgVenda.style.color = "#1bc5bd"; // Verde sucesso
                msgVenda.style.display = 'block';

                // Limpa o PDV para o próximo cliente
                carrinho = [];
                totalVenda = 0;
                document.getElementById('total-venda').innerText = "R$ 0,00";
                
                // Esconde a mensagem de sucesso após 3 segundos
                setTimeout(() => { msgVenda.style.display = 'none'; }, 3000);
            } else {
                // A sua regra de negócios C# travou a venda (Ex: Acabou o estoque!)
                const errorData = await resVenda.json();
                msgVenda.innerText = errorData.mensagem || "Erro ao registrar venda.";
                msgVenda.style.color = "#f1416c";
                msgVenda.style.display = 'block';
            }
        } catch (err) {
            msgVenda.innerText = "Erro de conexão com o servidor da API.";
            msgVenda.style.color = "#f1416c";
            msgVenda.style.display = 'block';
        }

        btnFinalizar.innerText = "Finalizar Venda";
    });

    // ==========================================
    // LÓGICA DO MODAL DO ADMINISTRADOR (MANTIDA INTACTA)
    // ==========================================
    const btnAdminDash = document.getElementById('btn-admin-dash');
    const modalAdmin = document.getElementById('modal-admin');
    const btnCancelarAdmin = document.getElementById('btn-cancelar-admin');
    const btnLogarAdmin = document.getElementById('btn-logar-admin');
    const msgErroAdmin = document.getElementById('msg-erro-admin');

    if(btnAdminDash) {
        btnAdminDash.addEventListener('click', function(e) {
            e.preventDefault();
            modalAdmin.style.display = 'flex';
        });
    }

    if(btnCancelarAdmin) {
        btnCancelarAdmin.addEventListener('click', function() {
            modalAdmin.style.display = 'none';
            msgErroAdmin.style.display = 'none';
        });
    }

    if(btnLogarAdmin) {
        btnLogarAdmin.addEventListener('click', async function() {
            const email = document.getElementById('email-admin').value;
            const senha = document.getElementById('senha-admin').value;

            if(!email || !senha) {
                msgErroAdmin.innerText = "Preencha os campos.";
                msgErroAdmin.style.display = 'block';
                return;
            }

            btnLogarAdmin.innerText = "Validando...";

            try {
                const response = await fetch('http://localhost:5062/api/auth/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email: email, senha: senha })
                });

                if(response.ok) {
                    const data = await response.json();
                    if(data.perfil === 'Admin' || data.perfil === 'Administrador' || data.perfil === 0 || data.perfil === '0') {
                        localStorage.setItem('authToken', data.token);
                        localStorage.setItem('userPerfil', data.perfil);
                        window.location.href = 'dashboard.html';
                    } else {
                        msgErroAdmin.innerText = "Acesso negado. Usuário não é Admin.";
                        msgErroAdmin.style.display = 'block';
                        btnLogarAdmin.innerText = "Liberar";
                    }
                } else {
                    msgErroAdmin.innerText = "Credenciais inválidas.";
                    msgErroAdmin.style.display = 'block';
                    btnLogarAdmin.innerText = "Liberar";
                }
            } catch (error) {
                msgErroAdmin.innerText = "Erro de conexão.";
                msgErroAdmin.style.display = 'block';
                btnLogarAdmin.innerText = "Liberar";
            }
        });
    }

    // ==========================================
    // LÓGICA DO BOTÃO DE SAIR PADRÃO (MANTIDA INTACTA)
    // ==========================================
    const btnSair = document.getElementById('btn-sair');
    if(btnSair) {
        btnSair.addEventListener('click', function(e) {
            e.preventDefault();
            localStorage.removeItem('authToken');
            localStorage.removeItem('userPerfil');
            window.location.href = 'index.html';
        });
    }
});