function addTrackInsertbtn() {
    const container = document.getElementById('tracks-container');

    // create input
    const input = document.createElement('input');
    input.type = 'text';
    input.name = 'tbxTrackNames';
    input.placeholder = 'Track Name';
    input.className = 'form-input';

    // create delete button
    const deleteBtn = document.createElement('button');
    deleteBtn.type = 'button';
    deleteBtn.textContent = 'Delete';
    deleteBtn.className = 'btn btn-delete';
    deleteBtn.onclick = () => {
        container.removeChild(input);
        container.removeChild(deleteBtn);
        container.removeChild(document.createElement('br'));
    };
    //append buttons
    const br = document.createElement('br');
    container.appendChild(br);
    container.appendChild(input);
    container.appendChild(deleteBtn);
}