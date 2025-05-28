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

        const chatSessionId = document.getElementById("chatSessionId").value;
        const messageTextInput = document.getElementById("messageText");
        const sendButton = chatForm.querySelector('button[type="submit"]');

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

            if (response.ok) {

                messageTextInput.value = "";
                const messages = await response.json();

                if (messages.length > 0 && messages[0].chatSessionId) {
                    document.getElementById("chatSessionId").value = messages[0].chatSessionId;
                }

                const wrapper = document.createElement('div');
                wrapper.classList.add("container-message");

                messages.forEach(msg => {
                    const container = document.createElement('div');
                    container.classList.add("message");
                    container.classList.add(msg.role == 0 ? "user" : "assistant");

                    const messageContent = document.createElement('div');
                    messageContent.classList.add("message-content");
                    messageContent.innerHTML = msg.content;

                    container.appendChild(messageContent);
                    wrapper.appendChild(container);

                    document.getElementById("chatMessages").appendChild(wrapper);
                });

                chatMessages.scrollTop = chatMessages.scrollHeight;
                GenerateTitle();
            }
            else
            {
                try {
                    const errorData = await response.json();
                    const errorMessage = errorData.error || "Ocurrió un error desconocido al procesar el mensaje.";
                    alert(`Error: ${errorMessage}`);
                } catch (e) {
                    const errorText = await response.text();
                    alert(`Error inesperado: ${errorText || "No se pudo obtener información del error."}`);
                }
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Error al enviar el mensaje.");
        } finally {
            sendButton.disabled = false;
        }
    });
});

async function GenerateTitle() {

    const chatSessionId = document.getElementById("chatSessionId").value;
    console.log("chatSessionId que se enviará:", chatSessionId);

    try
    {
        const response = await fetch('/Chat/GenerateTitle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ chatSessionId })
        });


        if (response.ok) {

            const data = await response.json();
            const newTitle = data.title;

            const sessionLink = document.getElementById(`chat-title-${chatSessionId}`);
            const chatSessionList = document.querySelector(".chat-sessions-list");

            if (sessionLink) {
                sessionLink.textContent = newTitle;
            } else {
                const newLink = document.createElement('a');
                newLink.classList.add('chat-session-link');
                newLink.href = `/Chat/Index?chatSessionId=${chatSessionId}`;
                newLink.id = `chat-title-${chatSessionId}`;
                newLink.textContent = newTitle;

                if (chatSessionList) {
                    chatSessionList.prepend(newLink);
                }
            }         
        } else {
            console.error("Error en la respuesta del servidor:", response.statusText);
        }
    }
    catch (error)
    {
        console.error("Error:", error);
    }
}