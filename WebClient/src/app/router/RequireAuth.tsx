import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { useGlobalStore } from '../stores/globalStore'

export default function RequireAuth() {
  const { userStore } = useGlobalStore()
  const { isLoggedIn } = userStore
  const location = useLocation()

  if (!isLoggedIn) {
    return <Navigate to="/" state={{ from: location }} />
  }

  return <Outlet />
}
