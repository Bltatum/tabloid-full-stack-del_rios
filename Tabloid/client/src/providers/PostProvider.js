import React, { useState , useContext } from "react";
import {UserProfileContext} from "./UserProfileProvider"
import "firebase/auth";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [posts, setPosts] = useState([]);

  const apiUrl = "/api/post";
  const { getToken } = useContext(UserProfileContext);


  const getAllPosts = () => 
      getToken().then((token) =>
       fetch(apiUrl, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`
    }
   }) .then((res) => res.json())
      .then(setPosts));

  const addPost = (post) => 
  getToken().then((token) =>
    fetch(apiUrl, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(post),
    }).then(resp => {
      if (resp.ok) {
        return resp.json();
      }
      throw new Error("Unauthorized");
    }));

//   const searchPosts = (search) => {
//     return fetch(`api/post/search?q=${search}`)
//   .then(res => res.json())
//   .then(setPosts)
//   };

  const getPost = (id) => 
  getToken().then((token)=>
  fetch(`/api/post/${id}`, {
    method: "Get",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  }).then(resp => {
    if (resp.ok) {
      return resp.json();
    }
    throw new Error("Unauthorized");
  }));

const getUserPost = (id) => 
getToken().then((token)=>
  fetch(`/api/post/getbyuser/${id}`, {
    method: "Get",
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  }).then(resp => {
    if (resp.ok) {
      return resp.json();
    }
    throw new Error("Unauthorized");
  }));
  


  return (
    <PostContext.Provider value={{ posts, getAllPosts, addPost, searchPosts, getPost, getUserPost }}>
      {props.children}
    </PostContext.Provider>
  );
};