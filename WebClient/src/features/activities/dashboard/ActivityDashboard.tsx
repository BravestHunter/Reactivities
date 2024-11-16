import { Grid, Loader } from 'semantic-ui-react'
import ActivityList from './ActivityList'
import { useStore } from '../../../app/stores/store'
import { observer } from 'mobx-react-lite'
import { useEffect, useState } from 'react'
import ActivityFilters from './ActivityFilters'
import InfiniteScroll from 'react-infinite-scroller'
import ActivityListItemPlaceholder from './ActivityListItemPlaceholder'
import PagingParams from '../../../app/models/pagingParams'

export default observer(function ActivityDashboard() {
  const { activityStore } = useStore()
  const { loadActivities, activityRegistry, setPagingParams, pageParams } =
    activityStore
  const [loadingNext, setLoadingNext] = useState(false)

  function handleGetNext() {
    setLoadingNext(true)
    setPagingParams(new PagingParams(pageParams!.currentPage + 1))
    loadActivities().then(() => setLoadingNext(false))
  }

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivities()
  }, [activityRegistry.size, loadActivities])

  return (
    <Grid>
      <Grid.Column width="10">
        {activityStore.loadingInitial && !loadingNext ? (
          <>
            <ActivityListItemPlaceholder />
            <ActivityListItemPlaceholder />
          </>
        ) : (
          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={
              !loadingNext &&
              !!pageParams &&
              pageParams.currentPage < pageParams.totalPages
            }
            initialLoad={false}
          >
            <ActivityList />
          </InfiniteScroll>
        )}
      </Grid.Column>
      <Grid.Column width="6">
        <ActivityFilters />
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  )
})
