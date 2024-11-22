import { Navigate, RouteObject, createBrowserRouter } from 'react-router-dom'
import HomePage from '../../features/home/HomePage'
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard'
import ActivityForm from '../../features/activities/form/ActivityForm'
import ActivityDetails from '../../features/activities/details/ActivityDetails'
import TestErrors from '../../features/errors/TestError'
import NotFound from '../../features/errors/NotFound'
import ServerError from '../../features/errors/ServerError'
import ProfilePage from '../../features/profiles/ProfilePage'
import RequireAuth from './RequireAuth'
import MainLayout from '../layout/MainLayout'

export const routes: RouteObject[] = [
  {
    element: <MainLayout />,
    children: [
      {
        element: <RequireAuth />,
        children: [
          {
            path: 'activities',
            element: <ActivityDashboard />,
          },
          {
            path: 'activities/:id',
            element: <ActivityDetails />,
          },
          {
            path: 'createActivity',
            element: <ActivityForm key="create" />,
          },
          {
            path: 'manage/:id',
            element: <ActivityForm key="manage" />,
          },
          {
            path: 'profiles/:username',
            element: <ProfilePage />,
          },
          {
            path: 'errors',
            element: <TestErrors />,
          },
        ],
      },
      {
        path: 'notfound',
        element: <NotFound />,
      },
      {
        path: 'servererror',
        element: <ServerError />,
      },
      {
        path: '*',
        element: <Navigate replace to="/notfound" />,
      },
    ],
  },
  {
    path: '',
    element: <HomePage />,
  },
]

export const router = createBrowserRouter(routes)
