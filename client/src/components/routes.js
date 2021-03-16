import { Route, Switch } from 'react-router-dom';
import { SignIn } from './Auth';

const RoutePaths = {
   Contacts: "/contacts",
   ContactEdit: "/contacts/edit/:id",
   ContactNew: "/contacts/new",
   SignIn: "/",
   Register: "/register/"
};

function Routes() {
   return(
      <Switch>
         <Route exact path={RoutePaths.SignIn} component={SignIn} />
         {/* <Route path={RoutePaths.Register} component={Register} /> */}
         {/* <DefaultLayout exact path={RoutePaths.Contacts} component={Contacts} />
         <DefaultLayout path={RoutePaths.ContactNew} component={ContactForm} />
         <DefaultLayout path={RoutePaths.ContactEdit} component={ContactForm} /> */}
         {/* <Route path='/error/:code?' component={ErrorPage} /> */}
      </Switch>
   );
}

// const DefaultLayout = ({ component: Component, ...rest }: { component, path, exact }) => (
//    <Route {...rest} render={props => (
//       AuthService.isSignedIn() ? (
//          <div>
//             <Header {...props} />
//             <div className="container">
//                   <Component {...props} />
//             </div>
//          </div>
//       ) : (
//             <Redirect to={{
//                   pathname: RoutePaths.SignIn,
//                   state: { from: props.location }
//             }} />
//          )
//    )} />
// );

export default Routes;