import { useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../services/api';
import axios from 'axios';
import { useCookies } from 'react-cookie';

export function SignIn(props) {
   const [state, setState] = useState({
      initialLoad: false,
      error: null
   });

   const [formData, setFormData] = useState({
      email: 'admin@app.com',
      password: 'Admin@123'
   });

   const [cookies, setCookie] = useCookies(['token']);

   const search = props.location.search;
   const params = new URLSearchParams(search);

   let initialLoadContent = null;
   if (state.initialLoad) {
      if (params.get('expired')) {
         initialLoadContent = <div className="alert alert-info" role="alert">
            <strong>Sesion Expired</strong> You need to sign in again.
               </div>
      }

      if (props.history.location.state && props.history.location.state.signedOut) {
         initialLoadContent = <div className="alert alert-info" role="alert">
            <strong>Signed Out</strong>
         </div>
      }
   }

   function handleSubmit(event) {
      event.preventDefault();

      axios.post('http://localhost:5000/api/auth/login', formData, {
         headers: {
            'Content-Type': 'application/json',
         }
      })
      .then(response => {
         console.log(response.data);
         setCookie('token', response.data.token, { path: '/' });
         props.history.push('/contacts');
      });         
   }

   function handleInputChange(event) {
      const { name, value } = event.target

      setFormData({
         ...formData,
         [name]: value
      });
   }

   return (
      <div className="auth">
         <form className="formAuth" onSubmit={handleSubmit}>
            <h2 className="formAuthHeading">Please sign in</h2>
            {initialLoadContent}
            {state.error &&
               <div className="alert alert-danger" role="alert">
                  {state.error}
               </div>
            }

            <div className="mb-3">
               <label>Email address</label>
               <input type="email" name="email" className="form-control form-control-danger" placeholder="Email address" onChange={handleInputChange} value={formData.email} />
            </div>

            <div>
               <label>Password</label>
               <input type="password" name="password" className="form-control" placeholder="Password" onChange={handleInputChange} value={formData.password} />
            </div>


            <button className="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
         </form>
         <div className="authEtc">
            <Link to="/register">Register</Link>
         </div>
      </div>
   );
}