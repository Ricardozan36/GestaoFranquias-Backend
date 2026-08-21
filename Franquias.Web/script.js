document.getElementById('btn-login').addEventListener('click', async function() {
    const email = document.getElementById('email').value;
    const senha = document.getElementById('senha').value;
    const mensagem = document.getElementById('mensagem-erro');
    const btn = document.getElementById('btn-login');

    // Esconde a mensagem de erro toda vez que clica
    mensagem.style.display = 'none';

    // Validação rápida
    if (!email || !senha) {
        mensagem.innerText = "Por favor, preencha e-mail e senha.";
        mensagem.style.display = 'block';
        return;
    }

    btn.innerText = "Autenticando...";

    try {
        // Bate na porta do seu AuthController
        const response = await fetch('http://localhost:5062/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email: email, senha: senha })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Salva a chave do cofre e o perfil no navegador
            localStorage.setItem('authToken', data.token);
            localStorage.setItem('userPerfil', data.perfil);

            // ==========================================
            // A TRAVA DE SEGURANÇA E ROTEAMENTO INTELIGENTE
            // ==========================================
            // Dependendo de como seu Enum está no C#, ele pode vir como 'Admin', 'Administrador' ou '0'. 
            // Cobrimos todas as bases para garantir.
            if (data.perfil === 'Admin' || data.perfil === 'Administrador' || data.perfil === 0 || data.perfil === '0') {
                
                // É o chefão. Vai para o Dashboard ver o dinheiro.
                window.location.href = 'dashboard.html'; 
                
            } else {
                
                // É a franquia operando. Vai para a Frente de Caixa.
                window.location.href = 'pdv.html'; 
                
            }
        } else {
            // Erro de senha ou usuário inativo (retornos do seu C#)
            const errorData = await response.json();
            mensagem.innerText = errorData.mensagem || "E-mail ou senha incorretos.";
            mensagem.style.display = 'block';
            btn.innerText = "Entrar no Sistema";
        }
    } catch (error) {
        // API Desligada
        mensagem.innerText = "Erro de conexão com o servidor. A API está ligada?";
        mensagem.style.display = 'block';
        btn.innerText = "Entrar no Sistema";
    }
});