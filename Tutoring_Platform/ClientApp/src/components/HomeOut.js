import React from "react";
/**
 * This function is created as a page for Navbrand link of the site
 * */
export function HomeOut() {
    //Image Source: https://media.istockphoto.com/id/999246336/photo/high-school-tutor-giving-male-student-one-to-one-tuition-at-desk.jpg?s=612x612&w=0&k=20&c=BslNF6nY6wEkIeGLpWmUGt4dKfs1ykvyOlJFof0xoZg=

    return (
        <div>
            <h3>Welcome to Tutoriaa</h3>
            <img src={require('./images/tutoring_image.jpg')} />
        </div>
    );
}
