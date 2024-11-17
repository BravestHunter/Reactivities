import axios, { AxiosError, AxiosResponse } from 'axios'
import { toast } from 'react-toastify'
import { router } from '../router/Routes'
import { store } from '../stores/store'
import User from '../models/user'
import { Profile } from '../models/profile'
import UserActivity from '../models/userActivity'
import Photo from '../models/photo'
import { sleep } from '../utils'
import PagedList from '../models/pagedist'
import AccessToken from '../models/accessToken'
import ActivityFormValues from '../models/forms/activityFormValues'
import RegisterRequest from '../models/requests/registerRequest'
import LoginRequest from '../models/requests/loginRequest'
import ActivityDto from '../models/dtos/activityDto'

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

axios.interceptors.response.use(
  async (response) => {
    if (import.meta.env.NODE_ENV === 'development') {
      // Fake delay
      await sleep(1000)
    }

    return response
  },
  (error: AxiosError) => {
    const { data, status, config, headers } = error.response as AxiosResponse

    switch (status) {
      case 400:
        if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
          router.navigate('/notfound')
        }
        if (data.errors) {
          const modalStateErrors = Object.values(data.errors)

          throw modalStateErrors.flat()
        } else {
          toast.error(data)
        }
        break

      case 401:
        if (
          headers['www-authenticate'].startsWith('Bearer error="invalid_token"')
        ) {
          store.userStore.logout()
          toast.error('Session Expired')
        }
        toast.error('Unauthorized')
        break

      case 403:
        toast.error('Forbidden')
        break

      case 404:
        router.navigate('/notfound')
        break

      case 500:
        store.commonStore.setServerError(data)
        router.navigate('/servererror')
        break
    }

    return Promise.reject(error)
  }
)

axios.defaults.baseURL = import.meta.env.VITE_API_URL
axios.defaults.withCredentials = true

const responseBody = <T>(response: AxiosResponse<T>) => response.data

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Account = {
  register: (request: RegisterRequest) =>
    requests.post<User>('/account/register', request),
  login: (request: LoginRequest) =>
    requests.post<User>('/account/login', request),
  refreshToken: () => requests.get<AccessToken>('/account/refreshToken'),
  current: () => requests.get<User>('/account'),
}

const Profiles = {
  get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
  update: (profile: Partial<Profile>) => requests.put('/profiles', profile),
  listActivities: (username: string, predicate: string) =>
    requests.get<UserActivity[]>(
      `/profiles/${username}/activities?predicate=${predicate}`
    ),
  listFollowings: (username: string, predicate: string) =>
    requests.get<Profile[]>(`/follow/${username}?predicate=${predicate}`),
  updateFollowing: (username: string) =>
    requests.post(`/follow/${username}`, {}),
  uploadPhoto: (file: Blob) => {
    let formData = new FormData()
    formData.append('File', file)

    return axios.post<Photo>('/photos', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
  },
  setMainPhoto: (id: number) => requests.post(`/photos/${id}/setmain`, {}),
  deletePhoto: (id: number) => requests.delete(`/photos/${id}`),
}

const Activities = {
  list: (params: URLSearchParams) =>
    axios
      .get<PagedList<ActivityDto>>('/activities', { params })
      .then(responseBody),
  details: (id: number) => requests.get<ActivityDto>(`/activities/${id}`),
  create: (activity: ActivityFormValues) =>
    requests.post<ActivityDto>('/activities', activity),
  update: (activity: ActivityFormValues) =>
    requests.put<ActivityDto>(`/activities/${activity.id}`, activity),
  delete: (id: number) => requests.delete<void>(`/activities/${id}`),
  updateAttendance: (id: number, attend: boolean) =>
    requests.post<void>(
      `/activities/${id}/updateAttendance?attend=${attend}`,
      {}
    ),
}

const agent = {
  Account,
  Profiles,
  Activities,
}

export default agent
