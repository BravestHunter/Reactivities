import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr'
import { makeAutoObservable, runInAction } from 'mobx'
import ChatComment from '../models/comment'
import { globalStore } from './globalStore'
import ActivityStore from './activityStore'

export default class CommentStore {
  comments: ChatComment[] = []
  private hubConnection: HubConnection | null = null

  private readonly activityStore: ActivityStore

  constructor(activityStore: ActivityStore) {
    this.activityStore = activityStore

    makeAutoObservable(this)
  }

  createHubConnection = (activityId: number) => {
    if (this.activityStore.selectedActivity) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(import.meta.env.VITE_CHAT_URL + '?activityId=' + activityId, {
          accessTokenFactory: () => globalStore.commonStore.token!,
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build()

      this.hubConnection.start().catch((error) => console.log(error))

      this.hubConnection.on('LoadComments', (comments: ChatComment[]) => {
        runInAction(() => {
          comments.forEach((comment) => {
            comment.createdAt = new Date(comment.createdAt)
          })
          this.comments = comments
        })
      })

      this.hubConnection.on('ReceiveComment', (comment: ChatComment) => {
        runInAction(() => {
          comment.createdAt = new Date(comment.createdAt)
          this.comments.unshift(comment)
        })
      })
    }
  }

  clearComments = () => {
    this.comments = []
    this.stopHubConnection()
  }

  addComment = async (values: any) => {
    values.activityId = this.activityStore.selectedActivity?.id

    try {
      await this.hubConnection?.invoke('SendComment', values)
    } catch (error) {
      console.log(error)
    }
  }

  private stopHubConnection = () => {
    this.hubConnection?.stop().catch((error) => console.log(error))
  }
}
