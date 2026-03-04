document.addEventListener('DOMContentLoaded', () => {
    window.addTrack = function() {
        const container = document.getElementById('new-tracks-container');

        const wrapper = document.createElement('div');
        wrapper.className = 'new-track-wrapper';

        const input = document.createElement('input');
        input.type = 'text';
        input.name = 'NewTrackNames'; // matches your [BindProperty]
        input.placeholder = 'Track Name';
        input.className = 'form-input';
        input.required = true;

        const deleteBtn = document.createElement('button');
        deleteBtn.type = 'button';
        deleteBtn.textContent = 'Delete';
        deleteBtn.className = 'btn btn-delete';
        deleteBtn.onclick = () => container.removeChild(wrapper);

        wrapper.appendChild(input);
        wrapper.appendChild(deleteBtn);

        container.appendChild(wrapper);
    };
});