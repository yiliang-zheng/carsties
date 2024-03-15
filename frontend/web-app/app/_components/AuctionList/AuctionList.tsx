"use client";
import React, { useMemo } from "react";
import { trpc } from "@/app/_trpc/client";
import { useShallow } from "zustand/react/shallow";
import { useParamsStore } from "@/app/_hooks/useParamsStore";

import AuctionCard from "@/app/_components/AuctionCard/AuctionCard";
import Pagination from "@/app/_components/Pagination/Pagniation";
import PageSize from "@/app/_components/PageSize/PageSize";
import Filter from "@/app/_components/Filters/Filter";
import EmptyList from "@/app/_components/EmptyList/EmptyList";

import type { PagedAuction, Auction } from "@/server/schemas/auction";

type Props = {
  initialAuctions: PagedAuction | undefined;
};

const AuctionList = ({ initialAuctions }: Props) => {
  const params = useParamsStore(
    useShallow((state) => ({
      pageNumber: state.pageNumber,
      pageSize: state.pageSize,
      searchTerm: state.searchTerm,
      filterBy: state.filterBy,
      orderBy: state.orderBy,
    }))
  );
  const setParams = useParamsStore((state) => state.setParams);

  const { data, isLoading, isError } = trpc.auctions.list.useQuery(
    {
      pageNumber: params.pageNumber,
      pageSize: params.pageSize,
      searchTerm: params.searchTerm,
      filterBy: params.filterBy,
      orderBy: params.orderBy,
    },
    {
      initialData: initialAuctions,
      refetchOnMount: false,
      refetchOnReconnect: false,
      refetchOnWindowFocus: false,
    }
  );

  const auctions = useMemo(() => {
    if (!data) return [] as Auction[];

    return data.results;
  }, [data, isLoading]);

  const totalPages = useMemo(() => {
    if (!data) return 0;

    return data.pageCount;
  }, [data]);

  return (
    <div className="w-full flex flex-col lg:flex-row">
      <Filter />
      {isLoading && <div>Loading...</div>}
      {!isLoading && !isError && !data.totalCount && <EmptyList showReset />}
      {!isLoading && !isError && data.totalCount > 0 && (
        <div>
          <div className="grid grid-cols-1 gap-6 auto-rows-max  lg:grid-cols-3">
            {auctions.map((auction, idx) => (
              <div key={idx} className="w-full">
                <AuctionCard auction={auction} />
              </div>
            ))}
          </div>
          <div className="w-full flex justify-between my-2">
            <div></div>
            <Pagination
              currentPage={params.pageNumber}
              onPageChange={(pageNumber) => setParams({ pageNumber })}
              totalPages={totalPages}
            />
            <PageSize
              pageSize={params.pageSize}
              onPageSizeChange={(newPageSize) =>
                setParams({ pageSize: newPageSize })
              }
            />
          </div>
        </div>
      )}
    </div>
  );
};

AuctionList.displayName = "AuctionList";
export default AuctionList;
