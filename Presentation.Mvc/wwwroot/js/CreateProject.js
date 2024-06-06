const formProject = document.getElementById("create-project-form");
const tagAddInput = document.getElementById("tag-add-input");
const tagList = document.getElementById("tag-list");
const submitButton = document.getElementById("submit-create-project");
const allTags = document.getElementById("all-tags");

formProject.onsubmit = (ev) => {
    ev.preventDefault();
}

submitButton.onclick = () => {
    formProject.submit();
}

tagAddInput.onkeydown = (key) => {
    if (key.key !== "Enter"){
        return;
    }
    key.preventDefault();

    if (tagAddInput.value === ""){
        return;
    }

    const alphanumericRegex = /^[0-9a-z]+$/i;

    if (!alphanumericRegex.test(tagAddInput.value)) {
        alert("Only alphanumerical tags are allowed!");
        return;
    }
    
    allTags.value = "";
    
    for(let i = 0; i < tagList.children.length; i++){
        let elem = tagList.children.item(i);
        if (elem.innerText === tagAddInput.value){
            return;
        }
    }
    
    let newChild = document.createElement("span");
    newChild.innerText = tagAddInput.value;
    newChild.onclick = () => {
        tagList.removeChild(newChild);
    };
    
    tagAddInput.value = "";

    tagList.appendChild(newChild);
    
    for(let i = 0; i < tagList.children.length; i++){
        allTags.value += tagList.children.item(i).innerText;
        if (i < tagList.children.length-1){
            allTags.value += "|";
        }
    }
}