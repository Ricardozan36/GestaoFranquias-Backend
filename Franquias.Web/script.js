document.getElementById('btn-login').addEventListener('click', async function() {
    // 1. Pega os valores que o usuário digitou nas caixas de texto
    const email = document.getElementById('email').value;
    const senha = document.getElementById('senha').value;
    const mensagemErro = document.getElementById('mensagem-erro');

    // Esconde a mensagem de erro sempre que tentar um novo login
    mensagemErro.style.display = 'none';

    // Validação simples
    if (!email || !senha) {
        mensagemErro.innerText = "Por favor, preencha todos os campos.";
        mensagemErro.style.display = 'block';
        return;
    }

    // Muda o texto do botão para dar um feedback visual
    const btn = document.getElementById('btn-login');
    btn.innerText = "Carregando...";

    try {
        // 2. Bate na porta da sua API (Back-End)
        const response = await fetch('http://localhost:5062/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: email, senha: senha })
        });

        // 3. Analisa a resposta da API
        if (response.ok) {
            const result = await response.json();
            
            // Sucesso! Guarda o Token no "cofre" do navegador (LocalStorage)
            localStorage.setItem('authToken', result.token);
            
            // Redireciona para a página do Dashboard (que vamos criar na sequência)
            window.location.href = 'dashboard.html';
        } else {
            // E-mail ou senha errados
            mensagemErro.innerText = "E-mail ou senha incorretos. Tente novamente.";
            mensagemErro.style.display = 'block';
            btn.innerText = "Entrar no Sistema";
        }
    } catch (error) {
        // Se cair aqui, a API provavelmente está desligada
        mensagemErro.innerText = "Erro de conexão. Verifique se a API está rodando.";
        mensagemErro.style.display = 'block';
        btn.innerText = "Entrar no Sistema";
    }
});