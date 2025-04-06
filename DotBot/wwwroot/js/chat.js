window.addEventListener('DOMContentLoaded', () => {
    const chatMessages = document.getElementById('chatMessages');
    if (chatMessages) {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    const chatForm = document.getElementById('chat-form');

    chatForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const messageText = document.getElementById("messageText").value;
        const chatSessionId = document.getElementById("chatSessionId").value;

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
                const html = await response.text();
                document.getElementById("chatMessages").innerHTML = html;
                document.getElementById("messageText").value = "";
                chatMessages.scrollTop = chatMessages.scrollHeight;
            } else {
                alert("Error al enviar el mensaje.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("Error al enviar el mensaje.");
        }
    });
});