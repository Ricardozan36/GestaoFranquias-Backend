document.getElementById('btn-cadastrar').addEventListener('click', async function() {
    // 1. Pega os dados digitados
    const nome = document.getElementById('nome-cad').value;
    const email = document.getElementById('email-cad').value;
    const senha = document.getElementById('senha-cad').value;
    const mensagem = document.getElementById('mensagem-cadastro');

    mensagem.style.display = 'none';

    // Validação básica
    if (!nome || !email || !senha) {
        mensagem.style.color = '#f1416c';
        mensagem.innerText = "Por favor, preencha todos os campos.";
        mensagem.style.display = 'block';
        return;
    }

    const btn = document.getElementById('btn-cadastrar');
    btn.innerText = "Salvando no banco...";

    try {
        // 2. Manda os dados para a sua API C# cadastrar no banco
        const response = await fetch('http://localhost:5062/api/usuarios', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            // AJUSTE CIRÚRGICO AQUI: Trocamos 'senha' por 'senhaHash' e adicionamos 'ativo: true'
            body: JSON.stringify({ 
                nome: nome, 
                email: email, 
                senhaHash: senha, 
                ativo: true 
            })
        });

        if (response.ok || response.status === 201) {
            // Sucesso
            mensagem.style.color = '#1bc5bd'; // Verde
            mensagem.innerText = "Cadastro realizado com sucesso! Redirecionando...";
            mensagem.style.display = 'block';
            
            // Espera 2 segundos e manda o usuário para a tela de Login
            setTimeout(() => {
                window.location.href = 'index.html';
            }, 2000);
        } else {
            // A API recusou (ex: email já existe)
            mensagem.style.color = '#f1416c'; // Vermelho
            mensagem.innerText = "Erro ao cadastrar. Verifique os dados.";
            mensagem.style.display = 'block';
            btn.innerText = "Finalizar Cadastro";
        }
    } catch (error) {
        // A API está desligada
        mensagem.style.color = '#f1416c';
        mensagem.innerText = "Erro de conexão com a API.";
        mensagem.style.display = 'block';
        btn.innerText = "Finalizar Cadastro";
    }
});