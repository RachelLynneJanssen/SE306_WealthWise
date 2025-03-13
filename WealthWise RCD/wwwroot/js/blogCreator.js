﻿
document.addEventListener("DOMContentLoaded", function () {
    // SAVING FUNCTIONALITY
    const options = {
        debug: 'info',
        modules: {
            toolbar: [
                [{ header: [1, 2, false] }],
                ['bold', 'italic', 'underline'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['clean'],
                ['save']
            ],
            history: true,
        },
        placeholder: 'Start writing here...',
        theme: 'snow'
    };
    const quill = new Quill('#editor', options);

    // change the tool bar styling after it is initialized
    setTimeout(() => {
        document.querySelector('.ql-toolbar').style.backgroundColor = '#C4C4A1';
    }, 100); // 100ms delay

    // Add Save button to toolbar
    const toolbar = quill.getModule('toolbar');
    const saveButton = document.createElement('button');
    saveButton.innerHTML = '<i class="fa-regular fa-floppy-disk" style="color: #3F3F2F;"></i>';
    saveButton.className = 'ql-save';
    saveButton.title = 'Save Post';
    saveButton.id = 'saveBtn';
    toolbar.container.appendChild(saveButton);

    // save the content when the save button is clicked
    saveButton.addEventListener('click', function () {
        const title = document.getElementById('blogTitle').value;
        const topic = document.getElementById('blogTopic').value;
        const content = quill.root.innerHTML;

        if (!title || !topic || !content) {
            alert('Please fill in all fields before saving.');
            return;
        }

        const blogData = {
            Title: title,
            Topic: topic,
            Content: content
        };

        fetch('/Advisor/LearningHub/SaveTempBlog', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Title: title.trim(),
                Topic: topic.trim(),
                Content: content.trim()
            })
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => { throw new Error(text); });
                }
                return response.text();
            })
            .then(data => {
                alert('Blog saved successfully.');

                // update the last saved time on the page
                const lastSavedTime = document.getElementById('lastSaved');
                if (lastSavedTime) {
                    const now = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                    lastSavedTime.textContent = `Last saved: ${now}`;
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert("Failed to save blog draft. Check the console for details.");
            });
    });

    document.getElementById('postButton').addEventListener('click', function () {
        const title = document.getElementById('blogTitle').value.trim();
        const topic = document.getElementById('blogTopic').value.trim();
        const content = quill.root.innerHTML.trim();

        if (!title || !topic || !content || content === "<p><br></p>") {
            alert('Please fill in all fields before posting.');
            return;
        }

        console.log("TITLE: ", title);
        console.log("TOPIC: ", topic);

        const blogData = {
            Title: title,
            Topic: topic,
            Content: content
        };

        fetch('/Advisor/LearningHub/PostBlog', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(blogData)
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text); });
            }
            return response.text();
        })
        .then(data => {
            alert('Blog posted successfully.');
            // take the user back to the blog home page
            window.location.href = '/Advisor/LearningHub/BlogPosts';
        })
        .catch((error) => {
            console.error('Error:', error);
            alert("Failed to post blog. Please try again.");
        });

    });
})
