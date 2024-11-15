import { makeAutoObservable, runInAction } from 'mobx'
import User, { AccessToken } from '../models/user'
import agent from '../api/agent'
import { store } from './store'
import { router } from '../router/Routes'
import LoginRequest from '../models/loginRequest'
import RegisterRequest from '../models/registerRequest'

export default class UserStore {
  user: User | null = null
  refreshTokenTimeout: any

  constructor() {
    makeAutoObservable(this)
  }

  get isLoggedIn() {
    return !!this.user
  }

  login = async (request: LoginRequest) => {
    try {
      const user = await agent.Account.login(request)
      runInAction(() => (this.user = user))

      await this.refreshToken()

      store.modalStore.closeModal()

      router.navigate('/activities')
    } catch (error) {
      throw error
    }
  }

  register = async (request: RegisterRequest) => {
    try {
      const user = await agent.Account.register(request)
      runInAction(() => (this.user = user))

      await this.refreshToken()

      store.modalStore.closeModal()

      router.navigate('/activities')
    } catch (error) {
      throw error
    }
  }

  logout = () => {
    store.commonStore.setToken(null)

    this.user = null

    router.navigate('/')
  }

  getUser = async () => {
    try {
      const user = await agent.Account.current()
      runInAction(() => (this.user = user))
    } catch (error) {
      console.log(error)
    }
  }

  setPhoto = (photoUrl: string) => {
    if (this.user) {
      this.user.profilePhotoUrl = photoUrl
    }
  }

  setDisplayName = (name: string) => {
    if (this.user) {
      this.user.displayName = name
    }
  }

  refreshToken = async () => {
    this.stopRefreshTokenTimer()

    try {
      const accessToken = await agent.Account.refreshToken()
      store.commonStore.setToken(accessToken.accessToken)
      this.startRefreshTokenTimer(accessToken)
    } catch (error) {
      console.log(error)
      this.logout()
    }
  }

  private startRefreshTokenTimer(accessToken: AccessToken) {
    const jwtToken = JSON.parse(atob(accessToken.accessToken.split('.')[1]))
    const expires = new Date(jwtToken.exp * 1000)
    const timeout = expires.getTime() - Date.now() - 60 * 1000
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout)
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout)
  }
}
