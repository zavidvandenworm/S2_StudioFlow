let nameHtml = document.getElementById("project-name");
let descriptionHtml = document.getElementById("project-description");
let btn = document.getElementById("completion-button");

let successToast = Toastify({
    text: "Rewrote project data!",
    duration: 3000,
    gravity: "bottom", // `top` or `bottom`
    position: "right", // `left`, `center` or `right`
    stopOnFocus: true, // Prevents dismissing of toast on hover
    style: {
        background: "linear-gradient(to right, #00b09b, #96c93d)",
    },
});

let failToast = Toastify({
    text: "Failed to rewrite project data.",
    duration: 3000,
    gravity: "bottom", // `top` or `bottom`
    position: "right", // `left`, `center` or `right`
    stopOnFocus: true, // Prevents dismissing of toast on hover
    style: {
        background: "rgb(255,200,200)",
    },
});

btn.onclick = () => {
    AiFill(nameHtml.value, descriptionHtml.value)
}

function AiFill(name, description){
    fetch("/completions/project?" + new URLSearchParams({
        title: name,
        description: description,
        tags: "tag"
    }))
        .then((response) => {
            response.text().then((text) => {
                let js = JSON.parse(text);
                nameHtml.value = js["title"];
                descriptionHtml.value = js["description"];
                successToast.showToast();
            })
        }).catch(() => {
            failToast.showToast();
    })
}