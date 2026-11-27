
(() => {
    const idleTimeout = 20 * 60 * 1000; // 20 minutes
    const heartbeatInterval = 4 * 60 * 1000; // 4 minutes

    let lastActivity = Date.now();

    const logoutForm = document.getElementById("autoLogoutForm");

    function updateActivity() {
        lastActivity = Date.now();
    }

    // User activity events
    [
        "mousemove",
        "mousedown",
        "keydown",
        "click",
        "scroll",
        "touchstart"
    ].forEach(eventName => {
        document.addEventListener(eventName, updateActivity, {
            passive: true
        });
    });

    // Check inactivity every second
    setInterval(() => {
        if (Date.now() - lastActivity >= idleTimeout) {
            logoutForm.submit();
        }
    }, 1000);

    // Keep authentication session alive while user is active
    setInterval(async () => {
        if (Date.now() - lastActivity < idleTimeout) {
            try {
                const response = await fetch(window.keepAliveUrl, {
                    method: "GET",
                    credentials: "same-origin",
                    cache: "no-store"
                });

                if (response.redirected) {
                    window.location.assign(response.url);
                }
            } catch (error) {
                console.error("KeepAlive request failed:", error);
            }
        }
    }, heartbeatInterval);
})();
