window.addEventListener('load', () => {
    const chatMessages = document.getElementById('chatMessages');

    if (chatMessages) {
        setTimeout(() => {
            chatMessages.scrollTop = chatMessages.scrollHeight;
        }, 0);
    }

    const chatForm = document.getElementById('chat-form');

    chatForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const messageTextInput = document.getElementById("messageText");
        const sendButton = chatForm.querySelector('button[type="submit"]');
        const chatSessionId = document.getElementById("chatSessionId").value;

        // Deshabilitar el botón para evitar doble envío
        sendButton.disabled = true;

        const messageText = messageTextInput.value;

        try {
            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Content: messageText,
                    ChatSessionId: chatSessionId
                })
            });

            messageTextInput.value = "";

            if (response.ok) {

                const messages = await response.json();
                console.log(messages);

                messages.forEach(msg => {
                    const div = document.createElement('div');
                    div.classList.add("message");
                    div.classList.add(msg.role == 0 ? "user" : "bot");

                    div.innerHTML = `<div>${msg.content}</div>`;

                    document.getElementById("chatMessages").appendChild(div);
                });

                chatMessages.scrollTop = chatMessages.scrollHeight;
            } else {
                alert("Error al enviar el mensaje.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Error al enviar el mensaje.");
        } finally {
            sendButton.disabled = false;
        }
    });
});